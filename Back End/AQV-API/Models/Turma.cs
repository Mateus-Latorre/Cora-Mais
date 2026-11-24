using System;
using System.Collections.Generic;

namespace AQV_API.Models;

public partial class Turma
{
    public Guid IdTurma { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
}
