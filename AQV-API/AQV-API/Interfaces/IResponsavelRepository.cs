using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IResponsavelRepository
    {
        void Cadastrar(Responsavel x);
        List<Responsavel> Listar();
        Responsavel BuscarPorId(Guid id);
        List<Responsavel> ListarPorAluno(Guid idAluno);
        void Atualizar(Guid id, Responsavel x);
        void Deletar(Guid id);
    }
}
