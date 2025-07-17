using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class POLISELER
{
    public int id { get; set; }

    public string police_no { get; set; } = null!;

    public int? teklif_id { get; set; }

    public int musteri_id { get; set; }

    public int police_turu_id { get; set; }

    public int sigorta_sirketi_id { get; set; }

    public int tanzim_eden_kullanici_id { get; set; }

    public string? risk_bilgileri { get; set; }

    public string? teminat_bilgileri { get; set; }

    public DateTime baslangic_tarihi { get; set; }

    public DateTime bitis_tarihi { get; set; }

    public decimal? brut_prim { get; set; }

    public decimal? net_prim { get; set; }

    public decimal? komisyon_tutari { get; set; }

    public decimal? vergi_tutari { get; set; }

    public decimal? toplam_tutar { get; set; }

    public int? taksit_sayisi { get; set; }

    public int durum_id { get; set; }

    public string? iptal_nedeni { get; set; }

    public DateTime? iptal_tarihi { get; set; }

    public DateTime? yenileme_hatirlatma_tarihi { get; set; }

    public string? ozel_sartlar { get; set; }

    public string? notlar { get; set; }

    public DateTime tanzim_tarihi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARs { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<KOMISYON_HESAPLARI> KOMISYON_HESAPLARIs { get; set; } = new List<KOMISYON_HESAPLARI>();

    public virtual ICollection<ODEMELER> ODEMELERs { get; set; } = new List<ODEMELER>();

    public virtual ICollection<TAKSITLER> TAKSITLERs { get; set; } = new List<TAKSITLER>();

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual MUSTERILER musteri { get; set; } = null!;

    public virtual POLICE_TURLERI police_turu { get; set; } = null!;

    public virtual SIGORTA_SIRKETLERI sigorta_sirketi { get; set; } = null!;

    public virtual KULLANICILAR tanzim_eden_kullanici { get; set; } = null!;

    public virtual POLICE_TEKLIFLERI? teklif { get; set; }
}
