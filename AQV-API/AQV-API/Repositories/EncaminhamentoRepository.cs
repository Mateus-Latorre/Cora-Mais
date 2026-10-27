using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV_API.Interfaces;

namespace AQV_API.Repositories
{
    public class EncaminhamentoRepository : IEncaminhamentoRepository
    {
        private readonly AqvContext _context;

        public EncaminhamentoRepository(AqvContext context)
        {
            _context = context;
        }

        /// <summary>Registra um novo encaminhamento com data atual e sem devolutiva.</summary>
        public void Cadastrar(Encaminhamento x)
        {
            x.IdEncaminhamento = Guid.NewGuid();
            x.DataEnvio = DateTime.Now;
            x.DataDevolutiva = null;
            _context.Encaminhamentos.Add(x);
            _context.SaveChanges();
        }

        public List<Encaminhamento> ListarPorOcorrencia(Guid idOcorrencia)
        {
            return _context.Encaminhamentos
                .Where(e => e.IdOcorrencia == idOcorrencia)
                .OrderBy(e => e.DataEnvio)
                .ToList();
        }

        /// <summary>Encaminhamento mais recente ainda sem devolutiva — indica quem está com o caso.</summary>
        public Encaminhamento BuscarPendentePorOcorrencia(Guid idOcorrencia)
        {
            return _context.Encaminhamentos
                .Where(e => e.IdOcorrencia == idOcorrencia && e.DataDevolutiva == null)
                .OrderByDescending(e => e.DataEnvio)
                .FirstOrDefault()!;
        }

        public Encaminhamento BuscarPorId(Guid id)
        {
            return _context.Encaminhamentos.FirstOrDefault(e => e.IdEncaminhamento == id)!;
        }

        /// <summary>
        /// O responsável ou coordenador devolve a ocorrência para a AQV:
        /// grava a data da devolutiva e a observação de retorno.
        /// </summary>
        public void RegistrarDevolutiva(Guid id, string? observacao)
        {
            var encaminhamento = BuscarPorId(id);
            if (encaminhamento != null)
            {
                encaminhamento.DataDevolutiva = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(observacao))
                {
                    encaminhamento.Observacao = observacao;
                }
                _context.Encaminhamentos.Update(encaminhamento);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var encaminhamento = BuscarPorId(id);
            if (encaminhamento != null)
            {
                _context.Encaminhamentos.Remove(encaminhamento);
                _context.SaveChanges();
            }
        }
    }
}
