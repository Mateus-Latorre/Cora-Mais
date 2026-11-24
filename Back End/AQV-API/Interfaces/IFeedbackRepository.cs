using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IFeedbackRepository
    {
        void Cadastrar(Feedback x);
        List<Feedback> ListarPorOcorrencia(Guid idOcorrencia);
        Feedback BuscarPorId(Guid id);
        void MarcarComoLido(Guid id);
        void Deletar(Guid id);
    }
}
