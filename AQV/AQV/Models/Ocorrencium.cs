using System;
using System.Collections.Generic;

namespace AQV.Models;

public partial class Ocorrencium
{
    public Guid IdOcorrencia { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public DateTime? DataAbertura { get; set; }

    public Guid? IdAluno { get; set; }

    public string? IdOstatus { get; set; }

    public Guid? IdUsuario { get; set; }

    public string Prioridade { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Aluno? IdAlunoNavigation { get; set; }

    public virtual Ostatus? IdOstatusNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
