using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class UsuarioCadastroDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O E-mail informado não é válido.")]
        [StringLength(256, ErrorMessage = "O E-mail deve ter no máximo 256 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "A Senha deve ter entre 6 e 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O Número deve ter entre 10 e 11 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo IdTipoUsuario é obrigatório.")]
        public Guid IdTipoUsuario { get; set; }
    }
}
