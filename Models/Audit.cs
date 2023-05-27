using System;
using System.Collections.Generic;

namespace LTMS.Models;

public partial class Audit
{
    public int Id { get; set; }

    public string? Bus { get; set; }

    public DateTime? DateAudit { get; set; }

    public string? NomAuditeur { get; set; }

    public string? PersonneAuditee { get; set; }

    public int Feux { get; set; }

    public int Maintenance { get; set; }

    public int Chaises { get; set; }

    public int Pneux { get; set; }

    public int Vitres { get; set; }

    public int Assurance { get; set; }

    public int CarteProfessionelle { get; set; }

    public int ContratLeoni { get; set; }

    public int Horraires { get; set; }

    public int Comportements { get; set; }

    public virtual Chauffeur? PersonneAuditeeNavigation { get; set; }
}
