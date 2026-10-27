using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Ostatus
{
    public string IdOstatus { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public virtual ICollection<Ocorrencia> Ocorrencia { get; set; } = new List<Ocorrencia>();
}
