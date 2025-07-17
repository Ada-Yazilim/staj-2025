using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class KOMISYON_HESAPLARI
{
    public int id { get; set; }

    public int police_id { get; set; }

    public int acente_kullanici_id { get; set; }

    public decimal? brut_prim { get; set; }

    public decimal? komisyon_orani { get; set; }

    public decimal? komisyon_tutari { get; set; }

    public decimal? stopaj_tutari { get; set; }

    public decimal? net_komisyon { get; set; }

    public int durum_id { get; set; }

    public DateTime hesaplama_tarihi { get; set; }

    public DateTime? odeme_tarihi { get; set; }

    public string? aciklama { get; set; }

    public virtual KULLANICILAR acente_kullanici { get; set; } = null!;

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual POLISELER police { get; set; } = null!;
}
