using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface INotificacaoRepository
    {
        void Cadastrar(Notificacao x);
        List<Notificacao> ListarPorUsuario(Guid idUsuario);
        Notificacao BuscarPorId(Guid id);
        IEnumerable<Notificacao> ListarPorOcorrencia(Guid idOcorrencia);
        void MarcarComoLida(Guid id);
        void Deletar(Guid id);
    }
}