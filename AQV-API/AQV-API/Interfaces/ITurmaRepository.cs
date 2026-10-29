using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface ITurmaRepository
    {
        void Cadastrar(Turma x);
        List<Turma> Listar();
        Turma BuscarPorId(Guid id);
        void Atualizar(Guid id, Turma x);
        void Deletar(Guid id);
    }
}
