using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class TurmaCadastroDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}
