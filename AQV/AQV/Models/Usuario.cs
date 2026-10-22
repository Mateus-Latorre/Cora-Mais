using System;
using System.Collections.Generic;

namespace AQV.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public Guid? IdTipoUsuario { get; set; }

    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    public virtual ICollection<Ocorrencium> Ocorrencia { get; set; } = new List<Ocorrencium>();
}
