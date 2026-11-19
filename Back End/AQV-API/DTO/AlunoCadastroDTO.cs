using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class AlunoCadastroDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O E-mail informado não é válido.")]
        [StringLength(256, ErrorMessage = "O E-mail deve ter no máximo 256 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres.")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        [StringLength(11, ErrorMessage = "O Número deve ter no máximo 11 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo IdTurma é obrigatório.")]
        public Guid IdTurma { get; set; }
    }
}
