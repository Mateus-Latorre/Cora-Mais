using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly IOcorrenciasRepository _OcorrenciasRepository;
        private readonly INotificacaoRepository _notificacaoRepository;

        public EncaminhamentoController(
            IEncaminhamentoRepository encaminhamentoRepository,
            IOcorrenciasRepository OcorrenciasRepository,
            INotificacaoRepository notificacaoRepository)
        {
            _encaminhamentoRepository = encaminhamentoRepository;
            _OcorrenciasRepository = OcorrenciasRepository;
            _notificacaoRepository = notificacaoRepository;
        }

        /// <summary>
        /// Histórico completo de encaminhamentos da ocorrência (linha do tempo da tratativa).
        /// </summary>
        [HttpGet("Ocorrencia/{idOcorrencias}")]
        public IActionResult ListarPorOcorrencias(Guid idOcorrencias)
        {
            try { return Ok(_encaminhamentoRepository.ListarPorOcorrencias(idOcorrencias)); }
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
        /// Registra a devolutiva do encaminhamento para a AQV.
        /// Se o destino foi um coordenador, somente o coordenador selecionado pode devolver.
        /// Se o destino foi o responsável do aluno, somente o professor que criou a ocorrência pode devolver.
        /// </summary>
        [HttpPatch("{id}/devolver")]
        [Authorize(Roles = "Professor,Coordenador")]
        public IActionResult Devolver(Guid id, [FromBody] string? observacao)
        {
            try
            {
                var encaminhamento = _encaminhamentoRepository.BuscarPorId(id);
                if (encaminhamento == null)
                {
                    return NotFound("Encaminhamento não encontrado.");
                }

                if (encaminhamento.DataDevolutiva != null)
                {
                    return BadRequest("Esta devolutiva já foi registrada.");
                }

                var idUsuarioLogado = ObterIdUsuarioLogado();
                if (idUsuarioLogado == Guid.Empty)
                {
                    return Unauthorized("Não foi possível identificar o usuário autenticado.");
                }

                // O SQL também possui uma restrição para garantir que apenas um destino seja informado.
                var encaminhadoParaCoordenador = encaminhamento.IdUsuarioCoordenador.HasValue;
                var encaminhadoParaResponsavel = encaminhamento.IdResponsavel.HasValue;

                if (encaminhadoParaCoordenador == encaminhadoParaResponsavel)
                {
                    return BadRequest("O encaminhamento não possui um único destino válido.");
                }

                if (encaminhadoParaCoordenador)
                {
                    // Encaminhamento para coordenador: somente o coordenador escolhido pode devolver.
                    if (!User.IsInRole("Coordenador") ||
                        encaminhamento.IdUsuarioCoordenador!.Value != idUsuarioLogado)
                    {
                        return Forbid();
                    }
                }
                else
                {
                    // Encaminhamento para responsável do aluno: o professor criador da ocorrência devolve.
                    if (!User.IsInRole("Professor"))
                    {
                        return Forbid();
                    }

                    var Ocorrencia = _OcorrenciasRepository.BuscarPorId(encaminhamento.IdOcorrencia);
                    if (Ocorrencia == null)
                    {
                        return NotFound("Ocorrência vinculada ao encaminhamento não encontrada.");
                    }

                    if (!Ocorrencia.IdUsuario.HasValue || Ocorrencia.IdUsuario.Value != idUsuarioLogado)
                    {
                        return Forbid();
                    }
                }

                _encaminhamentoRepository.RegistrarDevolutiva(id, observacao);

                // Após a devolutiva, a ocorrência retorna ao fluxo de acompanhamento da AQV.
                _OcorrenciasRepository.DefinirDestinoAtual(
                    encaminhamento.IdOcorrencia, null, null, "FINALIZADA");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Lê o identificador do usuário autenticado a partir das claims do JWT.</summary>
        private Guid ObterIdUsuarioLogado()
        {
            var claim = User.FindFirst(JwtRegisteredClaimNames.Jti)
                     ?? User.FindFirst("id")
                     ?? User.FindFirst(ClaimTypes.NameIdentifier);

            return Guid.TryParse(claim?.Value, out var id) ? id : Guid.Empty;
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
