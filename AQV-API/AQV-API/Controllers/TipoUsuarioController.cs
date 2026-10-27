using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AQV_API.Interfaces;
using AQV_API.DTO;
using AQV.WebAPI.Models;

namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AQV")]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public TipoUsuarioController(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] TipoUsuarioCadastroDTO dto)
        {
            try
            {
                var tipo = new TipoUsuario { IdTipoUsuario = Guid.NewGuid(), Titulo = dto.Titulo };
                _tipoUsuarioRepository.Cadastrar(tipo);
                return StatusCode(201, tipo);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista os tipos de usuário (usado para montar o select do cadastro).</summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Listar()
        {
            try { return Ok(_tipoUsuarioRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var tipo = _tipoUsuarioRepository.BuscarPorId(id);
                if (tipo == null) return NotFound("Tipo de usuário não encontrado.");
                return Ok(tipo);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(Guid id, [FromBody] TipoUsuarioCadastroDTO dto)
        {
            try
            {
                _tipoUsuarioRepository.Atualizar(id, new TipoUsuario { Titulo = dto.Titulo });
                return StatusCode(204);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(Guid id)
        {
            try
            {
                _tipoUsuarioRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }

}
