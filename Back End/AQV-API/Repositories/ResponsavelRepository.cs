using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AQV_API.Repositories
{
    public class ResponsavelRepository : IResponsavelRepository
    {
        private readonly AqvContext _context;

        public ResponsavelRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(Responsavel x)
        {
            _context.Responsavels.Add(x);
            _context.SaveChanges();
        }

        public List<Responsavel> Listar()
        {
            return _context.Responsavels
                .Include(r => r.IdAlunoNavigation)
                .OrderBy(r => r.Nome)
                .ToList();
        }

        public Responsavel BuscarPorId(Guid id)
        {
            return _context.Responsavels
                .Include(r => r.IdAlunoNavigation)
                .FirstOrDefault(r => r.IdResponsavel == id)!;
        }

        public List<Responsavel> ListarPorAluno(Guid idAluno)
        {
            return _context.Responsavels
                .Where(r => r.IdAluno == idAluno)
                .OrderBy(r => r.Nome)
                .ToList();
        }

        public void Atualizar(Guid id, Responsavel x)
        {
            var responsavelEncontrado = BuscarPorId(id);
            if (responsavelEncontrado != null)
            {
                responsavelEncontrado.Nome = x.Nome;
                responsavelEncontrado.Email = x.Email;
                responsavelEncontrado.Numero = x.Numero;
                responsavelEncontrado.IdAluno = x.IdAluno;

                _context.Responsavels.Update(responsavelEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var responsavelEncontrado = BuscarPorId(id);
            if (responsavelEncontrado != null)
            {
                _context.Responsavels.Remove(responsavelEncontrado);
                _context.SaveChanges();
            }
        }
    }
}
