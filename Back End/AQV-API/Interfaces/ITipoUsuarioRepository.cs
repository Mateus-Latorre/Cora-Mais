using AQV.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace AQV_API.Interfaces
{
    public interface ITipoUsuarioRepository
    {
        void Cadastrar(TipoUsuario x);
        List<TipoUsuario> Listar();
        TipoUsuario BuscarPorId(Guid id);
        TipoUsuario BuscarPorTitulo(string titulo);
        void Atualizar(Guid id, TipoUsuario x);
        void Deletar(Guid id);
    }
}
