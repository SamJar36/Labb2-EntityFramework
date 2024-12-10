using System;
using System.Collections.Generic;

namespace Labb2_EntityFramework.Model;

public partial class Kunder
{
    public string Personnummer { get; set; } = null!;

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public virtual ICollection<BonuspoängKonto> BonuspoängKontos { get; set; } = new List<BonuspoängKonto>();
}
