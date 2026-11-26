using AQV_API.Models;

namespace AQV_API.Interfaces
{
    public interface IEncaminhamentoRepository
    {
        void Cadastrar(Encaminhamento x);
        /// <summary>Histórico de encaminhamentos da ocorrência, do mais antigo ao mais novo.</summary>
        List<Encaminhamento> ListarPorOcorrencias(Guid idOcorrencias);
        /// <summary>Encaminhamento mais recente ainda sem devolutiva (quem está com o caso).</summary>
        Encaminhamento BuscarPendentePorOcorrencias(Guid idOcorrencias);
        Encaminhamento BuscarPorId(Guid id);
        /// <summary>Registra a devolutiva do responsável/coordenador para a AQV.</summary>
        void RegistrarDevolutiva(Guid id, string? observacao);
        void Deletar(Guid id);
    }
}
