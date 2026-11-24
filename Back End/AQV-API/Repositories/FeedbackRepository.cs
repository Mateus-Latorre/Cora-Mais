using AQV_API.BdContext;
using AQV_API.Interfaces;
using AQV_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AQV_API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AqvContext _context;

        public FeedbackRepository(AqvContext context)
        {
            _context = context;
        }

        // ==========================================
        // CADASTRAR
        // ==========================================

        public void Cadastrar(Feedback x)
        {
            _context.Feedbacks.Add(x);
            _context.SaveChanges();
        }

        // ==========================================
        // LISTAR POR OCORRÊNCIA
        // ==========================================

        public List<Feedback> ListarPorOcorrencia(Guid idOcorrencia)
        {
            return _context.Feedbacks
                .Where(f => f.IdOcorrencia == idOcorrencia)
                .ToList();
        }

        // ==========================================
        // BUSCAR POR ID
        // ==========================================

        public Feedback BuscarPorId(Guid id)
        {
            return _context.Feedbacks.Find(id)!;
        }

        // ==========================================
        // VERIFICAR SE JÁ EXISTE FEEDBACK
        // ==========================================

        public bool ExisteParaOcorrencia(Guid idOcorrencia)
        {
            return _context.Feedbacks
                .Any(f => f.IdOcorrencia == idOcorrencia);
        }

        // ==========================================
        // MARCAR COMO LIDO
        // ==========================================

        public void MarcarComoLido(Guid id)
        {
            var feedback = _context.Feedbacks.Find(id);

            if (feedback == null)
                return;

            feedback.Lido = true;

            _context.SaveChanges();
        }

        // ==========================================
        // DELETAR
        // ==========================================

        public void Deletar(Guid id)
        {
            var feedback = _context.Feedbacks.Find(id);

            if (feedback == null)
                return;

            _context.Feedbacks.Remove(feedback);
            _context.SaveChanges();
        }
    }
}