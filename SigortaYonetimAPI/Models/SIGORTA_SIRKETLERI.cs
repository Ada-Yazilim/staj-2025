using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class SIGORTA_SIRKETLERI
{
    public int id { get; set; }

    public string sirket_adi { get; set; } = null!;

    public string sirket_kodu { get; set; } = null!;

    public string? vergi_no { get; set; }

    public string? telefon { get; set; }

    public string? eposta { get; set; }

    public string? adres { get; set; }

    public bool aktif_mi { get; set; }

    public decimal? komisyon_orani { get; set; }

    public DateTime? sozlesme_baslangic { get; set; }

    public DateTime? sozlesme_bitis { get; set; }

    public virtual ICollection<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; } = new List<POLICE_TEKLIFLERI>();

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();
}
