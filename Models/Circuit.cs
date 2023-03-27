using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class Circuit
{
    public string? RefSapLeoni { get; set; }

    public int? NbKm { get; set; }

    public int? ContributionEmployé { get; set; }

    public int? CoutKm { get; set; }

    public string? PointArrivée { get; set; }

    public string? Agence { get; set; }

    public int Id { get; set; }

    public virtual Agence? AgenceNavigation { get; set; }
}
