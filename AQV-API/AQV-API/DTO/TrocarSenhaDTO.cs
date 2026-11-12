using System.ComponentModel.DataAnnotations;

namespace AQV_API.DTO
{
    /// <summary>Troca de senha da tela de Perfil: usuário logado informa a atual e a nova.</summary>
    public class TrocarSenhaDTO
    {
        [Required(ErrorMessage = "Informe a senha atual.")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a nova senha.")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "A nova senha deve ter entre 6 e 60 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "A confirmação não confere com a nova senha.")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}
