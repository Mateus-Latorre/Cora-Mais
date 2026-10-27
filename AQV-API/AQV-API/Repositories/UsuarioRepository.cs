using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV.WebAPI.Utils;
using AQV_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AQV_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AqvContext _context;

        public UsuarioRepository(AqvContext context)
        {
            _context = context;
        }

        /// <summary>Cadastra um usuário gerando o hash BCrypt da senha.</summary>
        public void Cadastrar(Usuario x)
        {
            x.Senha = Criptografia.GerarHash(x.Senha);
            _context.Usuarios.Add(x);
            _context.SaveChanges();
        }

        public List<Usuario> Listar()
        {
            return _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .OrderBy(u => u.Nome)
                .ToList();
        }

        public Usuario BuscarPorId(Guid id)
        {
            return _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefault(u => u.IdUsuario == id)!;
        }

        /// <summary>
        /// Busca o usuário pelo e-mail e valida a senha comparando o hash.
        /// Retorna null quando o e-mail não existe ou a senha não confere.
        /// </summary>
        public Usuario BuscarPorEmailESenha(string email, string senha)
        {
            var usuarioBuscado = _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefault(u => u.Email == email);

            if (usuarioBuscado != null && Criptografia.CompararHash(senha, usuarioBuscado.Senha))
            {
                return usuarioBuscado;
            }

            return null!;
        }

        /// <summary>Lista usuários por tipo — usado para montar a lista de coordenadores no encaminhamento.</summary>
        public List<Usuario> ListarPorTipoUsuario(string tituloTipoUsuario)
        {
            return _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .Where(u => u.IdTipoUsuarioNavigation!.Titulo == tituloTipoUsuario)
                .OrderBy(u => u.Nome)
                .ToList();
        }

        /// <summary>Atualiza os dados do usuário; a senha só muda se vier preenchida (re-hash).</summary>
        public void Atualizar(Guid id, Usuario x)
        {
            var usuarioEncontrado = BuscarPorId(id);
            if (usuarioEncontrado != null)
            {
                usuarioEncontrado.Nome = x.Nome;
                usuarioEncontrado.Email = x.Email;
                usuarioEncontrado.Numero = x.Numero;
                usuarioEncontrado.IdTipoUsuario = x.IdTipoUsuario;

                if (!string.IsNullOrWhiteSpace(x.Senha))
                {
                    usuarioEncontrado.Senha = Criptografia.GerarHash(x.Senha);
                }

                _context.Usuarios.Update(usuarioEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var usuarioEncontrado = BuscarPorId(id);
            if (usuarioEncontrado != null)
            {
                _context.Usuarios.Remove(usuarioEncontrado);
                _context.SaveChanges();
            }
        }
    }
}
