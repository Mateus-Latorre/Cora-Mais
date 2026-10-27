using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IAlunoRepository
    {
        void Cadastrar(Aluno x);
        List<Aluno> Listar();
        Aluno BuscarPorId(Guid id);
        void Atualizar(Guid id, Aluno x);
        void Deletar(Guid id);
    }
}
