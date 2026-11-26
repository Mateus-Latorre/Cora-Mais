using AQV_API.Models;

namespace AQV_API.Interfaces
{
    public interface INotificacaoRepository
    {
        void Cadastrar(Notificacao x);
        List<Notificacao> ListarPorUsuario(Guid idUsuario);
        Notificacao BuscarPorId(Guid id);
        IEnumerable<Notificacao> ListarPorOcorrencias(Guid idOcorrencias);
        void MarcarComoLida(Guid id);
        void Deletar(Guid id);
    }
}