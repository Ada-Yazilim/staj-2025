using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class POLICE_TURLERI
{
    public int id { get; set; }

    public int kategori_id { get; set; }

    public int alt_kategori_id { get; set; }

    public string urun_adi { get; set; } = null!;

    public string urun_kodu { get; set; } = null!;

    public string? aciklama { get; set; }

    public bool? zorunlu_mi { get; set; }

    public decimal? min_tutar { get; set; }

    public decimal? max_tutar { get; set; }

    public int? min_sure_gun { get; set; }

    public int? max_sure_gun { get; set; }

    public string? risk_faktorleri { get; set; }

    public bool aktif_mi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual ICollection<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; } = new List<POLICE_TEKLIFLERI>();

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();

    public virtual DURUM_TANIMLARI alt_kategori { get; set; } = null!;

    public virtual DURUM_TANIMLARI kategori { get; set; } = null!;
}
