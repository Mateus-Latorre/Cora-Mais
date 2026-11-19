using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class NotificacaoCadastroDTO
    {
        [Required(ErrorMessage = "O campo IdUsuario é obrigatório.")]
        public Guid IdUsuario { get; set; }

        [Required(ErrorMessage = "O campo Mensagem é obrigatório.")]
        [StringLength(300, ErrorMessage = "A Mensagem deve ter no máximo 300 caracteres.")]
        public string Mensagem { get; set; } = string.Empty;

        public Guid? IdOcorrencia { get; set; }
    }
}