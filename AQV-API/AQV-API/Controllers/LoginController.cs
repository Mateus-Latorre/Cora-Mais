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
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica o usuário e devolve o token JWT (8 horas) com o perfil na claim Role.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UsuarioLoginDTO dto)
        {
            try
            {
                var usuario = _usuarioRepository.BuscarPorEmailESenha(dto.Email, dto.Senha);
                if (usuario == null)
                {
                    return NotFound("E-mail ou senha inválidos.");
                }

                var perfil = usuario.IdTipoUsuarioNavigation?.Titulo ?? string.Empty;

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, usuario.IdUsuario.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
                    new Claim(ClaimTypes.Name, usuario.Nome!),
                    new Claim(ClaimTypes.Role, perfil),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                };

                var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave Jwt:Key não foi configurada.");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "api_aqv",
                    audience: "api_aqv",
                    claims: claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    idUsuario = usuario.IdUsuario,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    perfil
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
