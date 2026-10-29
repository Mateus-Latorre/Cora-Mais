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
    public class DashboardController : ControllerBase
    {
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public DashboardController(IOcorrenciaRepository ocorrenciaRepository)
        {
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        /// <summary>
        /// Resumo por status e por prioridade (cards: Total, Abertas, Em andamento, Finalizadas).
        /// </summary>
        [HttpGet("resumo")]
        public IActionResult Resumo()
        {
            try
            {
                int abertas = _ocorrenciaRepository.ContarPorStatus("ABERTA");
                int emAndamento = _ocorrenciaRepository.ContarPorStatus("ANDAMENTO")
                                + _ocorrenciaRepository.ContarPorStatus("ENCAMINHADA");
                int finalizadas = _ocorrenciaRepository.ContarPorStatus("FINALIZADA");

                return Ok(new
                {
                    Total = abertas + emAndamento + finalizadas,
                    Abertas = abertas,
                    EmAndamento = emAndamento,
                    Finalizadas = finalizadas,
                    Urgentes = _ocorrenciaRepository.ContarPorPrioridade("Urgente"),
                    Importantes = _ocorrenciaRepository.ContarPorPrioridade("Importante"),
                    Rotina = _ocorrenciaRepository.ContarPorPrioridade("Rotina")
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
