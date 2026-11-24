using AQV.WebAPI.Models;

namespace AQV_API.Interfaces
{
    public interface IOcorrenciaRepository
    {
        void Cadastrar(Ocorrencia x);
        List<Ocorrencia> Listar();
        Ocorrencia BuscarPorId(Guid id);
        List<Ocorrencia> ListarPorAluno(Guid idAluno);
        List<Ocorrencia> ListarPorStatus(string idOstatus);
        List<Ocorrencia> ListarPorPrioridade(string prioridade);
        /// <summary>Ocorrências registradas por um usuário (visão do professor).</summary>
        List<Ocorrencia> ListarPorUsuario(Guid idUsuario);
        /// <summary>Ocorrências que estão atualmente com um responsável.</summary>
        List<Ocorrencia> ListarPorResponsavelAtual(Guid idResponsavel);
        /// <summary>Ocorrências que estão atualmente com um coordenador.</summary>
        List<Ocorrencia> ListarPorCoordenadorAtual(Guid idUsuario);
        int ContarPorStatus(string idOstatus);
        int ContarPorPrioridade(string prioridade);
        void Atualizar(Guid id, Ocorrencia x);
        /// <summary>Altera apenas o status — usado somente pela AQV.</summary>
        void AtualizarStatus(Guid id, string idOstatus);
        /// <summary>Define quem está com a ocorrência agora (responsável ou coordenador) e o status.</summary>
        void DefinirDestinoAtual(Guid id, Guid? idResponsavel, Guid? idCoordenador, string idOstatus);
        void Deletar(Guid id);
    }
}
