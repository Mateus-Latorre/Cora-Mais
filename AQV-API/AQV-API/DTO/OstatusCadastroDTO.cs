using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class OstatusCadastroDTO
    {
        [Required(ErrorMessage = "O campo IdOstatus é obrigatório.")]
        [StringLength(20, ErrorMessage = "O IdOstatus deve ter no máximo 20 caracteres.")]
        public string IdOstatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Título deve ter no máximo 50 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
    }
}
