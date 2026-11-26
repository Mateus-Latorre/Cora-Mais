using System;
using System.Collections.Generic;

namespace AQV_API.Models;

public partial class Feedback
{
    public Guid IdFeedback { get; set; }

    public string Mensagem { get; set; } = null!;

    public DateTime? DataEnvio { get; set; }

    public bool? Lido { get; set; }

    public Guid? IdOcorrencia { get; set; }

    public virtual Ocorrencia? IdOcorrenciasNavigation { get; set; }
}
