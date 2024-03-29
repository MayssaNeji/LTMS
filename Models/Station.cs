﻿using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class Station
{
    public string? ReferenceRegion { get; set; }

    public string RefSapLeoni { get; set; } = null!;

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public double? Rayon { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Employe> Employes { get; } = new List<Employe>();

    public virtual Circuit RefSapLeoniNavigation { get; set; } = null!;
}
