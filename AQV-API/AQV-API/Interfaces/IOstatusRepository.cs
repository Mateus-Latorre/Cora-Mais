using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IOstatusRepository
    {
        void Cadastrar(Ostatus x);
        List<Ostatus> Listar();
        Ostatus BuscarPorId(string id);
        void Atualizar(string id, Ostatus x);
        void Deletar(string id);
    }
}
