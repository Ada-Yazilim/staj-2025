using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class TAKSITLER
{
    public int id { get; set; }

    public int police_id { get; set; }

    public int taksit_no { get; set; }

    public DateTime vade_tarihi { get; set; }

    public decimal? ana_para { get; set; }

    public decimal? vergi_tutari { get; set; }

    public decimal toplam_tutar { get; set; }

    public int durum_id { get; set; }

    public DateTime? odeme_tarihi { get; set; }

    public decimal? gecikme_faizi { get; set; }

    public int? gecikme_gun_sayisi { get; set; }

    public int? hatirlatma_durumu_id { get; set; }

    public DateTime? son_hatirlatma_tarihi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual ICollection<ODEMELER> ODEMELERs { get; set; } = new List<ODEMELER>();

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual DURUM_TANIMLARI? hatirlatma_durumu { get; set; }

    public virtual POLISELER police { get; set; } = null!;
}
