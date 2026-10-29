using System;
using System.Collections.Generic;

namespace AQV.WebAPI.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public Guid? IdTipoUsuario { get; set; }

    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    public virtual ICollection<Ocorrencia> Ocorrencia { get; set; } = new List<Ocorrencia>();

    public virtual ICollection<Encaminhamento> EncaminhamentoIdUsuarioAqvNavigations { get; set; } = new List<Encaminhamento>();

    public virtual ICollection<Encaminhamento> EncaminhamentoIdUsuarioCoordenadorNavigations { get; set; } = new List<Encaminhamento>();
}
