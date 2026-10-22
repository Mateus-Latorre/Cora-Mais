using System;
using System.Collections.Generic;

namespace AQV.Models;

public partial class Responsavel
{
    public Guid IdResponsavel { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public Guid? IdAluno { get; set; }

    public virtual Aluno? IdAlunoNavigation { get; set; }
}
