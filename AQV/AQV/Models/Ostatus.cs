using System;
using System.Collections.Generic;

namespace AQV.Models;

public partial class Ostatus
{
    public string IdOstatus { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public virtual ICollection<Ocorrencium> Ocorrencia { get; set; } = new List<Ocorrencium>();
}
