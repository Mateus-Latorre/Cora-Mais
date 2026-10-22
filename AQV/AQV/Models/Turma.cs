using System;
using System.Collections.Generic;

namespace AQV.Models;

public partial class Turma
{
    public Guid IdTurma { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
}
