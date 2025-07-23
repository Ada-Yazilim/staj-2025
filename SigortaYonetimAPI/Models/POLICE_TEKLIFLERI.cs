using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class POLICE_TEKLIFLERI
{
    public int id { get; set; }

    public string teklif_no { get; set; } = null!;

    public int musteri_id { get; set; }

    public int police_turu_id { get; set; }

    public int sigorta_sirketi_id { get; set; }

    public int olusturan_kullanici_id { get; set; }

    public string? risk_bilgileri { get; set; }

    public string? teminat_bilgileri { get; set; }

    public decimal? brut_prim { get; set; }

    public decimal? net_prim { get; set; }

    public decimal? komisyon_tutari { get; set; }

    public decimal? vergi_tutari { get; set; }

    public decimal? toplam_tutar { get; set; }

    public int? taksit_sayisi { get; set; }

    public DateTime teklif_tarihi { get; set; }

    public DateTime? gecerlilik_tarihi { get; set; }

    public int durum_id { get; set; }

    public string? red_nedeni { get; set; }

    public DateTime? onay_tarihi { get; set; }

    public string? onaylayan_kullanici { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public string? notlar { get; set; }

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual MUSTERILER musteri { get; set; } = null!;

    public virtual KULLANICILAR olusturan_kullanici { get; set; } = null!;

    public virtual POLICE_TURLERI police_turu { get; set; } = null!;

    public virtual SIGORTA_SIRKETLERI sigorta_sirketi { get; set; } = null!;
}
