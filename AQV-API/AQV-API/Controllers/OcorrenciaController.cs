using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV.WebAPI.BdContext;
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
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly AqvContext _context;

        public OcorrenciaController(
            IOcorrenciaRepository ocorrenciaRepository,
            IEncaminhamentoRepository encaminhamentoRepository,
            INotificacaoRepository notificacaoRepository,
            AqvContext context)
        {
            _ocorrenciaRepository = ocorrenciaRepository;
            _encaminhamentoRepository = encaminhamentoRepository;
            _notificacaoRepository = notificacaoRepository;
            _context = context;
        }

        /// <summary>Registra uma nova ocorrência (professor).</summary>
        [HttpPost]
        [Authorize(Roles = "Professor")]
        public IActionResult Cadastrar([FromBody] OcorrenciaCadastroDTO dto)
        {
            try
            {
                // O usuário que registra vem do token
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

                // Busca o IdTipoUsuario da AQV
                var idTipoAqv = _context.TipoUsuarios
                    .Where(t => t.Titulo == "AQV")
                    .Select(t => t.IdTipoUsuario)
                    .FirstOrDefault();

                // Notifica todos os usuários da AQV
                if (idTipoAqv != Guid.Empty)
                {
                    var usuariosAqv = _context.Usuarios
                        .Where(u => u.IdTipoUsuario == idTipoAqv)
                        .ToList();

                    foreach (var usuario in usuariosAqv)
                    {
                        var notificacao = new AQV.WebAPI.Models.Notificacao
                        {
                            IdNotificacao = Guid.NewGuid(),
                            IdUsuario = usuario.IdUsuario,
                            Mensagem = $"Nova ocorrência registrada: '{ocorrencia.Titulo}'",
                            DataEnvio = DateTime.Now,
                            Lida = false,
                            IdOcorrencia = ocorrencia.IdOcorrencia
                        };
                        _notificacaoRepository.Cadastrar(notificacao);
                    }
                }

                return StatusCode(201, ocorrencia);
            }
            catch (Exception ex)
            {
                var causa = ex.InnerException?.InnerException?.Message
                         ?? ex.InnerException?.Message
                         ?? ex.Message;
                return BadRequest(new { erro = ex.Message, detalhe = causa });
            }
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

                // Notifica o destinatário (somente se for Coordenador — Responsável não tem usuário no sistema)
                if (dto.TipoDestino == "Coordenador")
                {
                    var notificacaoDestinatario = new AQV.WebAPI.Models.Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        IdUsuario = dto.IdDestino,
                        Mensagem = $"Ocorrência encaminhada para você: {ocorrencia.Titulo}",
                        DataEnvio = DateTime.Now,
                        Lida = false,
                        IdOcorrencia = id
                    };
                    _notificacaoRepository.Cadastrar(notificacaoDestinatario);
                }

                // Notifica o professor que registrou a ocorrência
                if (ocorrencia.IdUsuario.HasValue)
                {
                    var notificacaoProfessor = new AQV.WebAPI.Models.Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        IdUsuario = ocorrencia.IdUsuario.Value,
                        Mensagem = $"Sua ocorrência '{ocorrencia.Titulo}' foi encaminhada para {(dto.TipoDestino == "Responsavel" ? "um responsável" : "um coordenador")}.",
                        DataEnvio = DateTime.Now,
                        Lida = false,
                        IdOcorrencia = id
                    };
                    _notificacaoRepository.Cadastrar(notificacaoProfessor);
                }

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
            return User.FindFirst("role")?.Value ?? string.Empty;
        }
    }
}
