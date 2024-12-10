using System;
using System.Collections.Generic;

namespace Labb2_EntityFramework.Model;

public partial class BonuspoängKonto
{
    public string KontoId { get; set; } = null!;

    public string KundId { get; set; } = null!;

    public int Poäng { get; set; }

    public virtual Kunder Kund { get; set; } = null!;

    public virtual ICollection<Butiker> Butiks { get; set; } = new List<Butiker>();
}
