using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class FeedbackCadastroDTO
    {
        [Required(ErrorMessage = "O campo Mensagem é obrigatório.")]
        [StringLength(300, ErrorMessage = "A Mensagem deve ter no máximo 300 caracteres.")]
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>URL da imagem anexada ao feedback (opcional).</summary>
        [StringLength(255, ErrorMessage = "A URL do anexo deve ter no máximo 255 caracteres.")]
        [Url(ErrorMessage = "O Anexo deve ser uma URL válida.")]
        public string? Anexo { get; set; }

        [Required(ErrorMessage = "O campo IdOcorrencia é obrigatório.")]
        public Guid IdOcorrencia { get; set; }
    }
}
