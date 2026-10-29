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
    public class TurmaController : ControllerBase
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaController(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        [HttpPost]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Cadastrar([FromBody] TurmaCadastroDTO dto)
        {
            try
            {
                var turma = new Turma { IdTurma = Guid.NewGuid(), Nome = dto.Nome };
                _turmaRepository.Cadastrar(turma);
                return StatusCode(201, turma);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista as turmas (usado no filtro "Turma: Todas").</summary>
        [HttpGet]
        public IActionResult Listar()
        {
            try { return Ok(_turmaRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var turma = _turmaRepository.BuscarPorId(id);
                if (turma == null) return NotFound("Turma não encontrada.");
                return Ok(turma);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Atualizar(Guid id, [FromBody] TurmaCadastroDTO dto)
        {
            try
            {
                _turmaRepository.Atualizar(id, new Turma { Nome = dto.Nome });
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
                _turmaRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
