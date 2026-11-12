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
        /// <summary>Troca a senha do usuário validando a senha atual.</summary>
        void TrocarSenha(Guid id, string senhaAtual, string novaSenha);
        void Deletar(Guid id);
    }
}
