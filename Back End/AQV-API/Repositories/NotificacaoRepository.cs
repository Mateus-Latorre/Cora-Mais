using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AQV_API.Repositories
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly AqvContext _context;

        public NotificacaoRepository(AqvContext context)
        {
            _context = context;
        }

        public void Cadastrar(Notificacao x)
        {
            x.IdNotificacao = Guid.NewGuid();
            x.DataEnvio = DateTime.Now;
            x.Lida = false;
            _context.Notificacaos.Add(x);
            _context.SaveChanges();
        }

        public List<Notificacao> ListarPorUsuario(Guid idUsuario)
        {
            return _context.Notificacaos
                .Where(n => n.IdUsuario == idUsuario)
                .OrderByDescending(n => n.DataEnvio)
                .ToList();
        }

        public Notificacao BuscarPorId(Guid id)
        {
            return _context.Notificacaos.FirstOrDefault(n => n.IdNotificacao == id)!;
        }

        public void MarcarComoLida(Guid id)
        {
            var notificacao = _context.Notificacaos.Find(id);
            if (notificacao != null)
            {
                notificacao.Lida = true;
                _context.Notificacaos.Update(notificacao);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var notificacao = _context.Notificacaos.Find(id);
            if (notificacao != null)
            {
                _context.Notificacaos.Remove(notificacao);
                _context.SaveChanges();
            }
        }
    }
}