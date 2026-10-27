using AQV.WebAPI.Models;
using AQV_API.DTO;
using AQV_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AQV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Cadastra um novo usuário. O cadastro é aberto (tela "Cadastre-se");
        /// a senha é hasheada no repository e nunca é devolvida no JSON.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Cadastrar([FromBody] UsuarioCadastroDTO dto)
        {
            try
            {
                var usuario = new Usuario
                {
                    IdUsuario = Guid.NewGuid(),
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Senha = dto.Senha,
                    Numero = dto.Numero,
                    IdTipoUsuario = dto.IdTipoUsuario
                };
                _usuarioRepository.Cadastrar(usuario);
                return StatusCode(201, new { usuario.IdUsuario, usuario.Nome, usuario.Email });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Authorize(Roles = "AQV,Coordenador")]
        public IActionResult Listar()
        {
            try { return Ok(_usuarioRepository.Listar()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Busca um usuário pelo ID (tela de Perfil).</summary>
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            try
            {
                var usuario = _usuarioRepository.BuscarPorId(id);
                if (usuario == null) return NotFound("Usuário não encontrado.");
                return Ok(usuario);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        /// <summary>Lista os coordenadores — usado no select de encaminhamento da AQV.</summary>
        [HttpGet("Coordenadores")]
        [Authorize(Roles = "AQV")]
        public IActionResult ListarCoordenadores()
        {
            try { return Ok(_usuarioRepository.ListarPorTipoUsuario("Coordenador")); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(Guid id, [FromBody] UsuarioCadastroDTO dto)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Senha = dto.Senha,
                    Numero = dto.Numero,
                    IdTipoUsuario = dto.IdTipoUsuario
                };
                _usuarioRepository.Atualizar(id, usuario);
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
                _usuarioRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
