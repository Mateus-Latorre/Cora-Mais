using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV_API.BdContext;
using AQV_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcorrenciaController : ControllerBase
    {
        private readonly IOcorrenciasRepository _OcorrenciasRepository;
        private readonly IEncaminhamentoRepository _encaminhamentoRepository;
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly AqvContext _context;

        public OcorrenciaController(
            IOcorrenciasRepository OcorrenciasRepository,
            IEncaminhamentoRepository encaminhamentoRepository,
            INotificacaoRepository notificacaoRepository,
            AqvContext context)
        {
            _OcorrenciasRepository = OcorrenciasRepository;
            _encaminhamentoRepository = encaminhamentoRepository;
            _notificacaoRepository = notificacaoRepository;
            _context = context;
        }

        /// <summary>Registra uma nova ocorrência (professor).</summary>
        [HttpPost]
        [Authorize(Roles = "Professor")]
        public IActionResult Cadastrar([FromBody] OcorrenciasCadastroDTO dto)
        {
            try
            {
                // O usuário que registra vem do token
                var idUsuario = ObterIdUsuarioLogado();
                var Ocorrencia = new Ocorrencia
                {
                    IdOcorrencia = Guid.NewGuid(),
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    IdAluno = dto.IdAluno,
                    Prioridade = dto.Prioridade,
                    IdOstatus = "ABERTA",
                    IdUsuario = idUsuario
                };
                _OcorrenciasRepository.Cadastrar(Ocorrencia);

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
                        var notificacao = new AQV_API.Models.Notificacao
                        {
                            IdNotificacao = Guid.NewGuid(),
                            IdUsuario = usuario.IdUsuario,
                            Mensagem = $"registrou uma nova ocorrência: '{Ocorrencia.Titulo}'",
                            DataEnvio = DateTime.Now,
                            Lida = false,
                            IdOcorrencia = Ocorrencia.IdOcorrencia
                        };
                        _notificacaoRepository.Cadastrar(notificacao);
                    }
                }

                return StatusCode(201, Ocorrencia);
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
            try { return Ok(_OcorrenciasRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Detalhes da ocorrência (todas as telas de detalhe).</summary>
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var Ocorrencia = _OcorrenciasRepository.BuscarPorId(id);
                if (Ocorrencia == null) return NotFound("Ocorrência não encontrada.");
                return Ok(Ocorrencia);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("Aluno/{idAluno}")]
        public IActionResult ListarPorAluno(Guid idAluno)
        {
            try { return Ok(_OcorrenciasRepository.ListarPorAluno(idAluno)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("Status/{idOstatus}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult ListarPorStatus(string idOstatus)
        {
            try { return Ok(_OcorrenciasRepository.ListarPorStatus(idOstatus)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Filtro por prioridade (Urgente, Importante, Rotina).</summary>
        [HttpGet("Prioridade/{prioridade}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult ListarPorPrioridade(string prioridade)
        {
            try { return Ok(_OcorrenciasRepository.ListarPorPrioridade(prioridade)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Ocorrências registradas pelo usuário logado (visão do professor).</summary>
        [HttpGet("Minhas")]
        public IActionResult ListarMinhas()
        {
            try { return Ok(_OcorrenciasRepository.ListarPorUsuario(ObterIdUsuarioLogado())); }
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
                    return Ok(_OcorrenciasRepository.ListarPorCoordenadorAtual(id));
                }

                return Ok(_OcorrenciasRepository.Listar());
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
                var Ocorrencia = _OcorrenciasRepository.BuscarPorId(id);
                if (Ocorrencia == null) return NotFound("Ocorrência não encontrada.");

                var pendente = _encaminhamentoRepository.BuscarPendentePorOcorrencias(id);
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

                _OcorrenciasRepository.DefinirDestinoAtual(
                    id,
                    dto.TipoDestino == "Responsavel" ? dto.IdDestino : null,
                    dto.TipoDestino == "Coordenador" ? dto.IdDestino : null,
                    "ANDAMENTO");

                // Notifica o destinatário (somente se for Coordenador — Responsável não tem usuário no sistema)
                if (dto.TipoDestino == "Coordenador")
                {
                    var notificacaoDestinatario = new AQV_API.Models.Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        IdUsuario = dto.IdDestino,
                        Mensagem = $"Ocorrência encaminhada para você: {Ocorrencia.Titulo}",
                        DataEnvio = DateTime.Now,
                        Lida = false,
                        IdOcorrencia = id
                    };
                    _notificacaoRepository.Cadastrar(notificacaoDestinatario);
                }

                // Notifica o professor que registrou a ocorrência
                if (Ocorrencia.IdUsuario.HasValue)
                {
                    var notificacaoProfessor = new AQV_API.Models.Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        IdUsuario = Ocorrencia.IdUsuario.Value,
                        Mensagem = $"Sua ocorrência '{Ocorrencia.Titulo}' foi encaminhada para {(dto.TipoDestino == "Responsavel" ? "um responsável" : "um coordenador")}.",
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
        /// <summary>Atualiza título, descrição, prioridade (AQV).</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "AQV")]
        public IActionResult Atualizar(Guid id, [FromBody] OcorrenciasAtualizadaDTO dto)
        {
            try
            {
                var Ocorrencia = new Ocorrencia
                {
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao,
                    Prioridade = dto.Prioridade
                };
                _OcorrenciasRepository.Atualizar(id, Ocorrencia);
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
                _OcorrenciasRepository.AtualizarStatus(id, idOstatus);
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
                _OcorrenciasRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                var detalhe = ex.GetBaseException().Message;

                return BadRequest(new
                {
                    erro = ex.Message,
                    detalhe = detalhe
                });
            }
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
