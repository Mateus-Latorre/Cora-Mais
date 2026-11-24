using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;

namespace AQV_API.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly AqvContext _context;

        public TurmaRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(Turma x)
        {
            _context.Turmas.Add(x);
            _context.SaveChanges();
        }

        public List<Turma> Listar()
        {
            return _context.Turmas.OrderBy(t => t.Nome).ToList();
        }

        public Turma BuscarPorId(Guid id)
        {
            return _context.Turmas.FirstOrDefault(t => t.IdTurma == id)!;
        }

        public void Atualizar(Guid id, Turma x)
        {
            var turmaEncontrada = BuscarPorId(id);
            if (turmaEncontrada != null)
            {
                turmaEncontrada.Nome = x.Nome;
                _context.Turmas.Update(turmaEncontrada);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var turmaEncontrada = BuscarPorId(id);
            if (turmaEncontrada != null)
            {
                _context.Turmas.Remove(turmaEncontrada);
                _context.SaveChanges();
            }
        }
    }
}
