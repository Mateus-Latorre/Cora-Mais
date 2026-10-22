using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Aluno
{
    public Guid IdAluno { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public Guid? IdTurma { get; set; }

    public virtual Turma? IdTurmaNavigation { get; set; }

    public virtual ICollection<Ocorrencia> Ocorrencia { get; set; } = new List<Ocorrencia>();

    public virtual ICollection<Responsavel> Responsavels { get; set; } = new List<Responsavel>();
}
