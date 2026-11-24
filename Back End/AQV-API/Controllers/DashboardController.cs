using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AQV_API.DTO;
using AQV_API.Interfaces;
using AQV_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOcorrenciasRepository _OcorrenciasRepository;

        public DashboardController(IOcorrenciasRepository OcorrenciasRepository)
        {
            _OcorrenciasRepository = OcorrenciasRepository;
        }

        /// <summary>
        /// Resumo por status e por prioridade (cards: Total, Abertas, Em andamento, Finalizadas).
        /// </summary>
        [HttpGet("resumo")]
        public IActionResult Resumo()
        {
            try
            {
                int abertas = _OcorrenciasRepository.ContarPorStatus("ABERTA");
                int emAndamento = _OcorrenciasRepository.ContarPorStatus("ANDAMENTO");
                int finalizadas = _OcorrenciasRepository.ContarPorStatus("FINALIZADA");

                return Ok(new
                {
                    Total = abertas + emAndamento + finalizadas,
                    Abertas = abertas,
                    EmAndamento = emAndamento,
                    Finalizadas = finalizadas,
                    Urgentes = _OcorrenciasRepository.ContarPorPrioridade("Urgente"),
                    Importantes = _OcorrenciasRepository.ContarPorPrioridade("Importante"),
                    Rotina = _OcorrenciasRepository.ContarPorPrioridade("Rotina")
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
