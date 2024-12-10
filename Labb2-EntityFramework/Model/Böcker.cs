using System;
using System.Collections.Generic;

namespace Labb2_EntityFramework;

public partial class Böcker
{
    public string Isbn13 { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public string Språk { get; set; } = null!;

    public decimal Pris { get; set; }

    public int FörfattareId { get; set; }

    public virtual Författare Författare { get; set; } = null!;

    public virtual ICollection<Lagersaldo> Lagersaldos { get; set; } = new List<Lagersaldo>();
}
