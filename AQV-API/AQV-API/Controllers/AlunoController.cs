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
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoController(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Cadastrar([FromBody] AlunoCadastroDTO dto)
        {
            try
            {
                var aluno = new Aluno
                {
                    IdAluno = Guid.NewGuid(),
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Cpf = dto.CPF,
                    Numero = dto.Numero,
                    IdTurma = dto.IdTurma
                };
                _alunoRepository.Cadastrar(aluno);
                return StatusCode(201, aluno);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista os alunos (usado no select "Estudante" da nova ocorrência).</summary>
        [HttpGet]
        public IActionResult Listar()
        {
            try { return Ok(_alunoRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var aluno = _alunoRepository.BuscarPorId(id);
                if (aluno == null) return NotFound("Aluno não encontrado.");
                return Ok(aluno);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Atualizar(Guid id, [FromBody] AlunoCadastroDTO dto)
        {
            try
            {
                var aluno = new Aluno
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Cpf = dto.CPF,
                    Numero = dto.Numero,
                    IdTurma = dto.IdTurma
                };
                _alunoRepository.Atualizar(id, aluno);
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
                _alunoRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
