using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class OcorrenciaCadastroDTO
    {
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(55, ErrorMessage = "O Título deve ter no máximo 55 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(250, ErrorMessage = "A Descrição deve ter no máximo 250 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo IdAluno é obrigatório.")]
        public Guid IdAluno { get; set; }

        [Required(ErrorMessage = "O campo Prioridade é obrigatório.")]
        [RegularExpression("^(Urgente|Importante|Rotina)$",
            ErrorMessage = "A Prioridade deve ser Urgente, Importante ou Rotina.")]
        public string Prioridade { get; set; } = "Rotina";

        /// <summary>URL da imagem anexada à ocorrência (opcional).</summary>
        [StringLength(255, ErrorMessage = "A URL do anexo deve ter no máximo 255 caracteres.")]
        [Url(ErrorMessage = "O Anexo deve ser uma URL válida.")]
        public string? Anexo { get; set; }
    }
}
