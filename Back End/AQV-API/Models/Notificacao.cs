using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Notificacao
{
    public Guid IdNotificacao { get; set; }

    public Guid IdUsuario { get; set; }

    public string Mensagem { get; set; } = null!;

    public DateTime? DataEnvio { get; set; }

    public bool? Lida { get; set; }

    public Guid? IdOcorrencia { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual Ocorrencia? IdOcorrenciaNavigation { get; set; }
}
