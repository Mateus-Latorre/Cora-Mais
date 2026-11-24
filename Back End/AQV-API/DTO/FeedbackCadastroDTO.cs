namespace AQV_API.DTO
{
    public class FeedbackCadastroDTO
    {
        public Guid IdOcorrencia { get; set; }

        public string Mensagem { get; set; } = string.Empty;
    }
}