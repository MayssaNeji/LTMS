using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class Shift
{
    public string ReferenceShift { get; set; } = null!;

    public string? HeureDebutShift { get; set; }

    public string? HeureFinShift { get; set; }

    public virtual ICollection<Employe> Employes { get; } = new List<Employe>();
}
