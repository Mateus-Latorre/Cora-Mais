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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public FeedbackController(
            IFeedbackRepository feedbackRepository,
            IOcorrenciaRepository ocorrenciaRepository)
        {
            _feedbackRepository = feedbackRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        /// <summary>
        /// Registra um feedback na ocorrência (Anexo = URL de uma imagem).
        /// Somente a AQV dá o feedback final ao professor.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "AQV")]
        public IActionResult Cadastrar([FromBody] FeedbackCadastroDTO dto)
        {
            try
            {
                var feedback = new Feedback
                {
                    IdFeedback = Guid.NewGuid(),
                    Mensagem = dto.Mensagem,
                    Anexo = dto.Anexo,
                    IdOcorrencia = dto.IdOcorrencia
                };
                _feedbackRepository.Cadastrar(feedback);
                return StatusCode(201, feedback);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Histórico de feedbacks de uma ocorrência.</summary>
        [HttpGet("Ocorrencia/{idOcorrencia}")]
        public IActionResult ListarPorOcorrencia(Guid idOcorrencia)
        {
            try { return Ok(_feedbackRepository.ListarPorOcorrencia(idOcorrencia)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var feedback = _feedbackRepository.BuscarPorId(id);
                if (feedback == null) return NotFound("Feedback não encontrado.");
                return Ok(feedback);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Marca o feedback como lido (tela de Notificações).</summary>
        [HttpPatch("{id}/lido")]
        public IActionResult MarcarComoLido(Guid id)
        {
            try
            {
                _feedbackRepository.MarcarComoLido(id);
                return StatusCode(204);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Encerra a tratativa: feedback dado e ocorrência finalizada (AQV).</summary>
        [HttpPatch("{id}/finalizar")]
        [Authorize(Roles = "AQV")]
        public IActionResult Finalizar(Guid id)
        {
            try
            {
                var feedback = _feedbackRepository.BuscarPorId(id);
                if (feedback == null) return NotFound("Feedback não encontrado.");

                _ocorrenciaRepository.DefinirDestinoAtual(
                    feedback.IdOcorrencia!.Value, null, null, "FINALIZADA");

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
                _feedbackRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
