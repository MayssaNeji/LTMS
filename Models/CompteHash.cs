using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class CompteHash
{
    public string Login { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;
}
