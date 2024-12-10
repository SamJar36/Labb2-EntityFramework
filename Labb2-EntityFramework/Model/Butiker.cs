using System;
using System.Collections.Generic;

namespace Labb2_EntityFramework.Model;

public partial class Butiker
{
    public int Id { get; set; }

    public string Butiksnamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Ort { get; set; } = null!;

    public int Postadress { get; set; }

    public virtual ICollection<Lagersaldo> Lagersaldos { get; set; } = new List<Lagersaldo>();

    public virtual ICollection<BonuspoängKonto> Kontos { get; set; } = new List<BonuspoängKonto>();
}
