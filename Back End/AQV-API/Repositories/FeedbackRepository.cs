using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;

namespace AQV_API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AqvContext _context;

        public FeedbackRepository(AqvContext context)
        {
            _context = context;
        }

        /// <summary>Cadastra o feedback com data de envio atual e não lido.</summary>
        public void Cadastrar(Feedback x)
        {
            x.DataEnvio = DateTime.Now;
            x.Lido = false;
            _context.Feedbacks.Add(x);
            _context.SaveChanges();
        }

        public List<Feedback> ListarPorOcorrencia(Guid idOcorrencia)
        {
            return _context.Feedbacks
                .Where(f => f.IdOcorrencia == idOcorrencia)
                .OrderBy(f => f.DataEnvio)
                .ToList();
        }

        public Feedback BuscarPorId(Guid id)
        {
            return _context.Feedbacks.FirstOrDefault(f => f.IdFeedback == id)!;
        }

        public void MarcarComoLido(Guid id)
        {
            var feedbackEncontrado = _context.Feedbacks.Find(id);
            if (feedbackEncontrado != null)
            {
                feedbackEncontrado.Lido = true;
                _context.Feedbacks.Update(feedbackEncontrado);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var feedbackEncontrado = _context.Feedbacks.Find(id);
            if (feedbackEncontrado != null)
            {
                _context.Feedbacks.Remove(feedbackEncontrado);
                _context.SaveChanges();
            }
        }
    }
}
