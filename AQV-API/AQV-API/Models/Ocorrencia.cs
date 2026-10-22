using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Ocorrencia
{
    public Guid IdOcorrencia { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public DateTime? DataAbertura { get; set; }

    public Guid? IdAluno { get; set; }

    public string? IdOstatus { get; set; }

    public Guid? IdUsuario { get; set; }

    public string Prioridade { get; set; } = null!;

    public Guid? IdResponsavelAtual { get; set; }

    public Guid? IdCoordenadorAtual { get; set; }

    public virtual ICollection<Encaminhamento> Encaminhamentos { get; set; } = new List<Encaminhamento>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Aluno? IdAlunoNavigation { get; set; }

    public virtual Usuario? IdCoordenadorAtualNavigation { get; set; }

    public virtual Ostatus? IdOstatusNavigation { get; set; }

    public virtual Responsavel? IdResponsavelAtualNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
