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
    public class ResponsavelController : ControllerBase
    {
        private readonly IResponsavelRepository _responsavelRepository;

        public ResponsavelController(IResponsavelRepository responsavelRepository)
        {
            _responsavelRepository = responsavelRepository;
        }

        [HttpPost]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Cadastrar([FromBody] ResponsavelCadastroDTO dto)
        {
            try
            {
                var responsavel = new Responsavel
                {
                    IdResponsavel = Guid.NewGuid(),
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Numero = dto.Numero,
                    IdAluno = dto.IdAluno
                };
                _responsavelRepository.Cadastrar(responsavel);
                return StatusCode(201, responsavel);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Listar()
        {
            try { return Ok(_responsavelRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var responsavel = _responsavelRepository.BuscarPorId(id);
                if (responsavel == null) return NotFound("Responsável não encontrado.");
                return Ok(responsavel);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista os responsáveis de um aluno (select de encaminhamento da AQV).</summary>
        [HttpGet("Aluno/{idAluno}")]
        public IActionResult ListarPorAluno(Guid idAluno)
        {
            try { return Ok(_responsavelRepository.ListarPorAluno(idAluno)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Atualizar(Guid id, [FromBody] ResponsavelCadastroDTO dto)
        {
            try
            {
                var responsavel = new Responsavel
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Numero = dto.Numero,
                    IdAluno = dto.IdAluno
                };
                _responsavelRepository.Atualizar(id, responsavel);
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
                _responsavelRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
