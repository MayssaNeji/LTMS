using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class Segment
{
    public string Nom { get; set; } = null!;

    public string CentreDeCout { get; set; } = null!;

    public string NomSegSapRef { get; set; } = null!;

    public string RhSegment { get; set; } = null!;

    public string ChefDeSegment { get; set; } = null!;

    public int Id { get; set; }
}
