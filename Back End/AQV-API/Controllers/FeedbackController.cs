using AQV_API.BdContext;
using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AqvContext _context;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly INotificacaoRepository _notificacaoRepository;

        public FeedbackController(
            AqvContext context,
            IFeedbackRepository feedbackRepository,
            INotificacaoRepository notificacaoRepository)
        {
            _context = context;
            _feedbackRepository = feedbackRepository;
            _notificacaoRepository = notificacaoRepository;
        }

        // ==========================================
        // LISTAR FEEDBACKS DA OCORRÊNCIA
        // ==========================================

        [HttpGet("Ocorrencia/{idOcorrencia}")]
        public IActionResult ListarPorOcorrencia(Guid idOcorrencia)
        {
            try
            {
                var feedbacks =
                    _feedbackRepository.ListarPorOcorrencia(idOcorrencia);

                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // BUSCAR FEEDBACK POR ID
        // ==========================================

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var feedback = _feedbackRepository.BuscarPorId(id);

                if (feedback == null)
                    return NotFound("Feedback não encontrado.");

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // CADASTRAR FEEDBACK
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "AQV")]
        public IActionResult Cadastrar([FromBody] FeedbackCadastroDTO dto)
        {
            try
            {
                // 1. Verifica se a ocorrência existe
                var ocorrencia = _context.Ocorrencias
                    .Find(dto.IdOcorrencia);

                if (ocorrencia == null)
                {
                    return NotFound("Ocorrência não encontrada.");
                }

                // 2. Só permite feedback após a devolutiva
                // A devolutiva altera o status para FINALIZADA
                if (ocorrencia.IdOstatus != "FINALIZADA")
                {
                    return Conflict(
                        "Só é possível cadastrar um feedback após a finalização da ocorrência."
                    );
                }

                // 3. Verifica se já existe um feedback
                if (_feedbackRepository.ExisteParaOcorrencia(dto.IdOcorrencia))
                {
                    return Conflict(
                        "Esta ocorrência já possui um feedback."
                    );
                }

                // 4. Cria o feedback
                var feedback = new Feedback
                {
                    IdFeedback = Guid.NewGuid(),
                    Mensagem = dto.Mensagem,
                    IdOcorrencia = dto.IdOcorrencia
                };

                // 5. Salva o feedback e atualiza o status juntos
                _context.Feedbacks.Add(feedback);

                ocorrencia.IdOstatus = "FEEDBACK";

                _context.SaveChanges();

                // 6. Notifica o usuário que criou a ocorrência
                if (ocorrencia.IdUsuario.HasValue)
                {
                    var notificacao = new Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        IdUsuario = ocorrencia.IdUsuario.Value,

                        Mensagem =
                            $"Novo feedback na ocorrência '{ocorrencia.Titulo}': {dto.Mensagem}",

                        DataEnvio = DateTime.Now,
                        Lida = false,
                        IdOcorrencia = dto.IdOcorrencia
                    };

                    _notificacaoRepository.Cadastrar(notificacao);
                }

                // 7. Retorna sucesso
                return StatusCode(201, new
                {
                    mensagem = "Feedback cadastrado com sucesso!",
                    statusOcorrencia = "FEEDBACK",
                    feedback = feedback
                });
            }
            catch (DbUpdateException ex)
                when (ex.GetBaseException() is SqlException sqlEx &&
                      (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return Conflict(
                    "Esta ocorrência já possui um feedback."
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // MARCAR COMO LIDO
        // ==========================================

        [HttpPatch("{id}/lido")]
        public IActionResult MarcarComoLido(Guid id)
        {
            try
            {
                var feedback = _feedbackRepository.BuscarPorId(id);

                if (feedback == null)
                    return NotFound("Feedback não encontrado.");

                _feedbackRepository.MarcarComoLido(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // DELETAR FEEDBACK
        // ==========================================

        [HttpDelete("{id}")]
        [Authorize(Roles = "AQV")]
        public IActionResult Deletar(Guid id)
        {
            try
            {
                var feedback = _feedbackRepository.BuscarPorId(id);

                if (feedback == null)
                    return NotFound("Feedback não encontrado.");

                _feedbackRepository.Deletar(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}