using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace AQV_API.DTO
{
    public class TipoUsuarioCadastroDTO
    {
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
    }
}
