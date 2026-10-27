using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Feedback
{
    public Guid IdFeedback { get; set; }

    public string Mensagem { get; set; } = null!;

    public DateTime? DataEnvio { get; set; }

    public bool? Lido { get; set; }

    public string? Anexo { get; set; }

    public Guid? IdOcorrencia { get; set; }

    public virtual Ocorrencia? IdOcorrenciaNavigation { get; set; }
}
