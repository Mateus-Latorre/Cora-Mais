using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IUsuarioRepository
    {
        void Cadastrar(Usuario x);
        List<Usuario> Listar();
        Usuario BuscarPorId(Guid id);
        Usuario BuscarPorEmailESenha(string email, string senha);
        /// <summary>Lista usuários de um tipo (ex.: "Coordenador") — usado no encaminhamento.</summary>
        List<Usuario> ListarPorTipoUsuario(string tituloTipoUsuario);
        void Atualizar(Guid id, Usuario x);
        void Deletar(Guid id);
    }
}
