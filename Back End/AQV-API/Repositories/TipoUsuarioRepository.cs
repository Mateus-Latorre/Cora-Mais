using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;

namespace AQV_API.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly AqvContext _context;

        public TipoUsuarioRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(TipoUsuario x)
        {
            _context.TipoUsuarios.Add(x);
            _context.SaveChanges();
        }

        public List<TipoUsuario> Listar()
        {
            return _context.TipoUsuarios.OrderBy(t => t.Titulo).ToList();
        }

        public TipoUsuario BuscarPorId(Guid id)
        {
            return _context.TipoUsuarios.FirstOrDefault(t => t.IdTipoUsuario == id)!;
        }

        /// <summary>Busca o tipo de usuário pelo título (ex.: "Professor", "AQV", "Coordenador").</summary>
        public TipoUsuario BuscarPorTitulo(string titulo)
        {
            return _context.TipoUsuarios.FirstOrDefault(t => t.Titulo == titulo)!;
        }

        public void Atualizar(Guid id, TipoUsuario x)
        {
            var tipoEncontrado = BuscarPorId(id);
            if (tipoEncontrado != null)
            {
                tipoEncontrado.Titulo = x.Titulo;
                _context.TipoUsuarios.Update(tipoEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var tipoEncontrado = BuscarPorId(id);
            if (tipoEncontrado != null)
            {
                _context.TipoUsuarios.Remove(tipoEncontrado);
                _context.SaveChanges();
            }
        }
    }
}
