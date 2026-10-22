using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Encaminhamento
{
    public Guid IdEncaminhamento { get; set; }

    public Guid IdOcorrencia { get; set; }

    public Guid? IdResponsavel { get; set; }

    public Guid? IdUsuarioCoordenador { get; set; }

    public Guid IdUsuarioAqv { get; set; }

    public DateTime DataEnvio { get; set; }

    public DateTime? DataDevolutiva { get; set; }

    public string? Observacao { get; set; }

    public virtual Ocorrencia IdOcorrenciaNavigation { get; set; } = null!;

    public virtual Responsavel? IdResponsavelNavigation { get; set; }

    public virtual Usuario IdUsuarioAqvNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioCoordenadorNavigation { get; set; }
}
