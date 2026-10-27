using AQV.WebAPI.BdContext;
using AQV.WebAPI.Models;
using AQV.WebAPI.Utils;
using AQV_API.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace AQV_API.Repositories
{
    public class OcorrenciaRepository : IOcorrenciaRepository
    {
        private readonly AqvContext _context;

        public OcorrenciaRepository(AqvContext context)
        {
            _context = context;
        }

        /// <summary>Registra a ocorrência com data de abertura atual e status inicial ABERTA.</summary>
        public void Cadastrar(Ocorrencia x)
        {
            x.DataAbertura = DateTime.Now;
            if (string.IsNullOrWhiteSpace(x.IdOstatus))
            {
                x.IdOstatus = "ABERTA";
            }
            _context.Ocorrencia.Add(x);
            _context.SaveChanges();
        }

        public List<Ocorrencia> Listar()
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        public Ocorrencia BuscarPorId(Guid id)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefault(o => o.IdOcorrencia == id)!;
        }

        public List<Ocorrencia> ListarPorAluno(Guid idAluno)
        {
            return _context.Ocorrencia
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Where(o => o.IdAluno == idAluno)
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        public List<Ocorrencia> ListarPorStatus(string idOstatus)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Where(o => o.IdOstatus == idOstatus)
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        public List<Ocorrencia> ListarPorPrioridade(string prioridade)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Where(o => o.Prioridade == prioridade)
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        public List<Ocorrencia> ListarPorUsuario(Guid idUsuario)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Where(o => o.IdUsuario == idUsuario)
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        /// <summary>Ocorrências que estão agora com o responsável informado.</summary>
        public List<Ocorrencia> ListarPorResponsavelAtual(Guid idResponsavel)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Where(o => _context.Encaminhamentos.Any(e =>
                    e.IdOcorrencia == o.IdOcorrencia &&
                    e.IdResponsavel == idResponsavel &&
                    e.DataDevolutiva == null))
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        /// <summary>Ocorrências que estão agora com o coordenador informado.</summary>
        public List<Ocorrencia> ListarPorCoordenadorAtual(Guid idUsuario)
        {
            return _context.Ocorrencia
                .Include(o => o.IdAlunoNavigation)!.ThenInclude(a => a!.IdTurmaNavigation)
                .Include(o => o.IdOstatusNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Where(o => _context.Encaminhamentos.Any(e =>
                    e.IdOcorrencia == o.IdOcorrencia &&
                    e.IdUsuarioCoordenador == idUsuario &&
                    e.DataDevolutiva == null))
                .OrderByDescending(o => o.DataAbertura)
                .ToList();
        }

        public int ContarPorStatus(string idOstatus)
        {
            return _context.Ocorrencia.Count(o => o.IdOstatus == idOstatus);
        }

        public int ContarPorPrioridade(string prioridade)
        {
            return _context.Ocorrencia.Count(o => o.Prioridade == prioridade);
        }

        public void Atualizar(Guid id, Ocorrencia x)
        {
            var ocorrenciaEncontrada = _context.Ocorrencia.Find(id);
            if (ocorrenciaEncontrada != null)
            {
                ocorrenciaEncontrada.Titulo = x.Titulo;
                ocorrenciaEncontrada.Descricao = x.Descricao;

                if (x.IdAluno != Guid.Empty)
                {
                    ocorrenciaEncontrada.IdAluno = x.IdAluno;
                }

                if (!string.IsNullOrWhiteSpace(x.Prioridade))
                {
                    ocorrenciaEncontrada.Prioridade = x.Prioridade;
                }

                if (!string.IsNullOrWhiteSpace(x.Anexo))
                {
                    ocorrenciaEncontrada.Anexo = x.Anexo;
                }

                _context.Ocorrencia.Update(ocorrenciaEncontrada);
                _context.SaveChanges();
            }
        }

        /// <summary>Altera apenas o status. Regra de negócio: somente a AQV pode chamar isto.</summary>
        public void AtualizarStatus(Guid id, string idOstatus)
        {
            var ocorrenciaEncontrada = _context.Ocorrencia.Find(id);
            if (ocorrenciaEncontrada != null)
            {
                ocorrenciaEncontrada.IdOstatus = idOstatus;
                _context.Ocorrencia.Update(ocorrenciaEncontrada);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Grava com quem a ocorrência está agora (responsável OU coordenador, nunca os dois)
        /// e o status correspondente. Usado no encaminhamento e na devolutiva.
        /// </summary>
        public void DefinirDestinoAtual(Guid id, Guid? idResponsavel, Guid? idCoordenador, string idOstatus)
        {
            var ocorrenciaEncontrada = _context.Ocorrencia.Find(id);
            if (ocorrenciaEncontrada != null)
            {
                // Quem esta com a ocorrencia e controlado pela tabela Encaminhamento
                // (registros com DataDevolutiva nula). Aqui apenas o status muda.
                ocorrenciaEncontrada.IdOstatus = idOstatus;
                _context.Ocorrencia.Update(ocorrenciaEncontrada);
                _context.SaveChanges();
            }
        }

        public void Deletar(Guid id)
        {
            var ocorrenciaEncontrada = _context.Ocorrencia.Find(id);
            if (ocorrenciaEncontrada != null)
            {
                _context.Ocorrencia.Remove(ocorrenciaEncontrada);
                _context.SaveChanges();
            }
        }
    }
}
