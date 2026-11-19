using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AQV_API.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AqvContext _context;

        public AlunoRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(Aluno x)
        {
            _context.Alunos.Add(x);
            _context.SaveChanges();
        }

        public List<Aluno> Listar()
        {
            return _context.Alunos
                .Include(a => a.IdTurmaNavigation)
                .OrderBy(a => a.Nome)
                .ToList();
        }

        public Aluno BuscarPorId(Guid id)
        {
            return _context.Alunos
                .Include(a => a.IdTurmaNavigation)
                .FirstOrDefault(a => a.IdAluno == id)!;
        }

        public void Atualizar(Guid id, Aluno x)
        {
            var alunoEncontrado = BuscarPorId(id);
            if (alunoEncontrado != null)
            {
                alunoEncontrado.Nome = x.Nome;
                alunoEncontrado.Email = x.Email;
                alunoEncontrado.Cpf = x.Cpf;
                alunoEncontrado.Numero = x.Numero;
                alunoEncontrado.IdTurma = x.IdTurma;

                _context.Alunos.Update(alunoEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var alunoEncontrado = BuscarPorId(id);
            if (alunoEncontrado != null)
            {
                _context.Alunos.Remove(alunoEncontrado);
                _context.SaveChanges();
            }
        }
    }

}
