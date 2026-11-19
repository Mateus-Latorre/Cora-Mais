using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;

namespace AQV_API.Repositories
{
    public class OstatusRepository : IOstatusRepository
    {
        private readonly AqvContext _context;

        public OstatusRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(Ostatus x)
        {
            _context.Ostatuses.Add(x);
            _context.SaveChanges();
        }

        public List<Ostatus> Listar()
        {
            return _context.Ostatuses.OrderBy(o => o.Titulo).ToList();
        }

        public Ostatus BuscarPorId(string id)
        {
            return _context.Ostatuses.FirstOrDefault(o => o.IdOstatus == id)!;
        }

        public void Atualizar(string id, Ostatus x)
        {
            var statusEncontrado = BuscarPorId(id);
            if (statusEncontrado != null)
            {
                statusEncontrado.Titulo = x.Titulo;
                _context.Ostatuses.Update(statusEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(string id)
        {
            var statusEncontrado = BuscarPorId(id);
            if (statusEncontrado != null)
            {
                _context.Ostatuses.Remove(statusEncontrado);
                _context.SaveChanges();
            }
        }
    }
}
