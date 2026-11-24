using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoRepository _notificacaoRepository;

        public NotificacaoController(INotificacaoRepository notificacaoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
        }

        /// <summary>
        /// Cadastra uma nova notificação.
        /// POST /api/Notificacao
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Cadastrar([FromBody] NotificacaoCadastroDTO dto)
        {
            try
            {
                var notificacao = new AQV.WebAPI.Models.Notificacao
                {
                    IdNotificacao = Guid.NewGuid(),
                    IdUsuario = dto.IdUsuario,
                    Mensagem = dto.Mensagem,
                    DataEnvio = DateTime.Now,
                    Lida = false,
                    IdOcorrencia = dto.IdOcorrencia
                };
                _notificacaoRepository.Cadastrar(notificacao);
                return StatusCode(201, notificacao);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Lista as notificações de um usuário.
        /// GET /api/Notificacao/Usuario/{idUsuario}
        /// </summary>
        [HttpGet("Usuario/{idUsuario}")]
        public IActionResult ListarPorUsuario(Guid idUsuario)
        {
            try
            {
                var notificacoes = _notificacaoRepository.ListarPorUsuario(idUsuario);
                var naoLidas = notificacoes.Count(n => n.Lida == false);
                
                return Ok(new
                {
                    total = notificacoes.Count,
                    naoLidas = naoLidas,
                    itens = notificacoes
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Busca uma notificação pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var notificacao = _notificacaoRepository.BuscarPorId(id);
                if (notificacao == null) return NotFound("Notificação não encontrada.");
                return Ok(notificacao);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Marca uma notificação como lida.
        /// PATCH /api/Notificacao/{id}/lida
        /// </summary>
        [HttpPatch("{id}/lida")]
        public IActionResult MarcarComoLida(Guid id)
        {
            try
            {
                _notificacaoRepository.MarcarComoLida(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Marca todas as notificações de um usuário como lidas.
        /// PATCH /api/Notificacao/Usuario/{idUsuario}/lidas
        /// </summary>
        [HttpPatch("Usuario/{idUsuario}/lidas")]
        public IActionResult MarcarTodasComoLidas(Guid idUsuario)
        {
            try
            {
                var notificacoes = _notificacaoRepository.ListarPorUsuario(idUsuario);
                
                foreach (var n in notificacoes.Where(n => n.Lida == false))
                {
                    _notificacaoRepository.MarcarComoLida(n.IdNotificacao);
                }
                
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Deleta uma notificação pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "AQV")]
        public IActionResult Deletar(Guid id)
        {
            try
            {
                _notificacaoRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}