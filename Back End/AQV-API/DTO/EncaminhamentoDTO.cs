using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    public class EncaminhamentoDTO
    {
        [Required(ErrorMessage = "Informe o tipo de destino: 'Responsavel' ou 'Coordenador'.")]
        [RegularExpression("^(Responsavel|Coordenador)$",
            ErrorMessage = "O tipo de destino deve ser 'Responsavel' ou 'Coordenador'.")]
        public string TipoDestino { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo IdDestino é obrigatório.")]
        public Guid IdDestino { get; set; }

        [StringLength(300, ErrorMessage = "A observação deve ter no máximo 300 caracteres.")]
        public string? Observacao { get; set; }
    }
}
