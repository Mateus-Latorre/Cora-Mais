using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcorrenciaController : ControllerBase
    {
        private readonly IOcorrenciaRepository _ocorrenciaRepository;
        private readonly IEncaminhamentoRepository _encaminhamentoRepository;

        public OcorrenciaController(
            IOcorrenciaRepository ocorrenciaRepository,
            IEncaminhamentoRepository encaminhamentoRepository)
        {
            _ocorrenciaRepository = ocorrenciaRepository;
            _encaminhamentoRepository = encaminhamentoRepository;
        }

        /// <summary>Registra uma nova ocorrência (professor, AQV ou coordenação).</summary>
        [HttpPost]
        [Authorize(Roles = "Professor,AQV,Coordenador")]
        public IActionResult Cadastrar([FromBody] OcorrenciaCadastroDTO dto)
        {
            try
            {
                // O usuário que registra vem do token (não confie no corpo da requisição)
                var idUsuario = ObterIdUsuarioLogado();

                var ocorrencia = new Ocorrencia
                {
                    IdOcorrencia = Guid.NewGuid(),
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    IdAluno = dto.IdAluno,
                    Prioridade = dto.Prioridade,
                    Anexo = dto.Anexo,
                    IdOstatus = "ABERTA",
                    IdUsuario = idUsuario
                };
                _ocorrenciaRepository.Cadastrar(ocorrencia);
                return StatusCode(201, ocorrencia);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista todas as ocorrências (visão da AQV e da coordenação).</summary>
        [HttpGet]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Listar()
        {
            try { return Ok(_ocorrenciaRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Detalhes da ocorrência (todas as telas de detalhe).</summary>
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var ocorrencia = _ocorrenciaRepository.BuscarPorId(id);
                if (ocorrencia == null) return NotFound("Ocorrência não encontrada.");
                return Ok(ocorrencia);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("Aluno/{idAluno}")]
        public IActionResult ListarPorAluno(Guid idAluno)
        {
            try { return Ok(_ocorrenciaRepository.ListarPorAluno(idAluno)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("Status/{idOstatus}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult ListarPorStatus(string idOstatus)
        {
            try { return Ok(_ocorrenciaRepository.ListarPorStatus(idOstatus)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Filtro por prioridade (Urgente, Importante, Rotina).</summary>
        [HttpGet("Prioridade/{prioridade}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult ListarPorPrioridade(string prioridade)
        {
            try { return Ok(_ocorrenciaRepository.ListarPorPrioridade(prioridade)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Ocorrências registradas pelo usuário logado (visão do professor).</summary>
        [HttpGet("Minhas")]
        public IActionResult ListarMinhas()
        {
            try { return Ok(_ocorrenciaRepository.ListarPorUsuario(ObterIdUsuarioLogado())); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Ocorrências que estão agora comigo (responsável ou coordenador).</summary>
        [HttpGet("Comigo")]
        public IActionResult ListarComigo()
        {
            try
            {
                var id = ObterIdUsuarioLogado();
                var perfil = ObterPerfilUsuarioLogado();

                if (perfil == "Coordenador")
                {
                    return Ok(_ocorrenciaRepository.ListarPorCoordenadorAtual(id));
                }

                if (perfil == "Responsavel")
                {
                    return Ok(_ocorrenciaRepository.ListarPorResponsavelAtual(id));
                }

                return Ok(_ocorrenciaRepository.Listar());
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Encaminha a ocorrência para um responsável do aluno ou para um coordenador.
        /// Somente a AQV encaminha.
        /// </summary>
        [HttpPost("{id}/encaminhar")]
        [Authorize(Roles = "AQV")]
        public IActionResult Encaminhar(Guid id, [FromBody] EncaminhamentoDTO dto)
        {
            try
            {
                var ocorrencia = _ocorrenciaRepository.BuscarPorId(id);
                if (ocorrencia == null) return NotFound("Ocorrência não encontrada.");

                var pendente = _encaminhamentoRepository.BuscarPendentePorOcorrencia(id);
                if (pendente != null)
                {
                    return BadRequest("Esta ocorrência já está encaminhada e aguarda devolutiva.");
                }

                var encaminhamento = new Encaminhamento
                {
                    IdOcorrencia = id,
                    IdUsuarioAqv = ObterIdUsuarioLogado(),
                    Observacao = dto.Observacao
                };

                if (dto.TipoDestino == "Responsavel")
                {
                    encaminhamento.IdResponsavel = dto.IdDestino;
                }
                else
                {
                    encaminhamento.IdUsuarioCoordenador = dto.IdDestino;
                }

                _encaminhamentoRepository.Cadastrar(encaminhamento);

                _ocorrenciaRepository.DefinirDestinoAtual(
                    id,
                    dto.TipoDestino == "Responsavel" ? dto.IdDestino : null,
                    dto.TipoDestino == "Coordenador" ? dto.IdDestino : null,
                    "ENCAMINHADA");

                return StatusCode(201, encaminhamento);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Atualiza título, descrição, prioridade e anexo (AQV).</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "AQV")]
        public IActionResult Atualizar(Guid id, [FromBody] OcorrenciaAtualizadaDTO dto)
        {
            try
            {
                var ocorrencia = new Ocorrencia
                {
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    Prioridade = dto.Prioridade,
                    Anexo = dto.Anexo
                };
                _ocorrenciaRepository.Atualizar(id, ocorrencia);
                return StatusCode(204);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Altera o status da ocorrência. REGRA DE NEGÓCIO: somente o perfil AQV.
        /// </summary>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "AQV")]
        public IActionResult AtualizarStatus(Guid id, [FromBody] string idOstatus)
        {
            try
            {
                _ocorrenciaRepository.AtualizarStatus(id, idOstatus);
                return StatusCode(204);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "AQV")]
        public IActionResult Deletar(Guid id)
        {
            try
            {
                _ocorrenciaRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lê o Id do usuário logado a partir do token JWT.</summary>
        private Guid ObterIdUsuarioLogado()
        {
            var claim = User.FindFirst(JwtRegisteredClaimNames.Jti)
                     ?? User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim?.Value, out var id) ? id : Guid.Empty;
        }

        /// <summary>Lê o perfil (Role) do usuário logado a partir do token JWT.</summary>
        private string ObterPerfilUsuarioLogado()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }
}
