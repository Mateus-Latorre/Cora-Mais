using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface INotificacaoRepository
    {
        void Cadastrar(Notificacao x);
        List<Notificacao> ListarPorUsuario(Guid idUsuario);
        Notificacao BuscarPorId(Guid id);
        void MarcarComoLida(Guid id);
        void Deletar(Guid id);
    }
}