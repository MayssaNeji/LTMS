using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class User
{
    public int Matricule { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public byte[]? Avatar { get; set; }

    public int Tel { get; set; }

    public DateTime DateDeNaissance { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;
}
