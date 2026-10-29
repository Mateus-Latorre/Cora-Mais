using AQV_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncaminhamentoController : ControllerBase
    {
        private readonly IEncaminhamentoRepository _encaminhamentoRepository;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public EncaminhamentoController(
            IEncaminhamentoRepository encaminhamentoRepository,
            IOcorrenciaRepository ocorrenciaRepository)
        {
            _encaminhamentoRepository = encaminhamentoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        /// <summary>
        /// Histórico completo de encaminhamentos da ocorrência (linha do tempo da tratativa).
        /// </summary>
        [HttpGet("Ocorrencia/{idOcorrencia}")]
        public IActionResult ListarPorOcorrencia(Guid idOcorrencia)
        {
            try { return Ok(_encaminhamentoRepository.ListarPorOcorrencia(idOcorrencia)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var encaminhamento = _encaminhamentoRepository.BuscarPorId(id);
                if (encaminhamento == null) return NotFound("Encaminhamento não encontrado.");
                return Ok(encaminhamento);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>
        /// Devolutiva: o coordenador devolve a ocorrência para a AQV, ou a própria AQV
        /// registra a devolutiva após o contato com o responsável do aluno.
        /// </summary>
        [HttpPatch("{id}/devolver")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Devolver(Guid id, [FromBody] string? observacao)
        {
            try
            {
                var encaminhamento = _encaminhamentoRepository.BuscarPorId(id);
                if (encaminhamento == null) return NotFound("Encaminhamento não encontrado.");

                if (encaminhamento.DataDevolutiva != null)
                {
                    return BadRequest("Esta devolutiva já foi registrada.");
                }

                _encaminhamentoRepository.RegistrarDevolutiva(id, observacao);

                // A ocorrência volta para a AQV (sem destino atual)
                _ocorrenciaRepository.DefinirDestinoAtual(
                    encaminhamento.IdOcorrencia, null, null, "AGUARDANDO_FEEDBACK");

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
                _encaminhamentoRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
