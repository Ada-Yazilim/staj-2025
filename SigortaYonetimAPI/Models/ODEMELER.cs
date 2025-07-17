using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class ODEMELER
{
    public int id { get; set; }

    public int? taksit_id { get; set; }

    public int? police_id { get; set; }

    public int musteri_id { get; set; }

    public int? tahsilat_yapan_kullanici_id { get; set; }

    public int? odeme_tipi_id { get; set; }

    public DateTime odeme_tarihi { get; set; }

    public decimal tutar { get; set; }

    public int? odeme_yontemi_id { get; set; }

    public int durum_id { get; set; }

    public string? banka_adi { get; set; }

    public string? kart_son_4_hane { get; set; }

    public string? pos_referans_no { get; set; }

    public string? banka_referans_no { get; set; }

    public string? makbuz_no { get; set; }

    public decimal? komisyon_tutari { get; set; }

    public string? aciklama { get; set; }

    public DateTime? tahsil_tarihi { get; set; }

    public DateTime? muhasebe_kayit_tarihi { get; set; }

    public bool? muhasebede_kaydedildi_mi { get; set; }

    public string? fis_no { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual MUSTERILER musteri { get; set; } = null!;

    public virtual DURUM_TANIMLARI? odeme_tipi { get; set; }

    public virtual DURUM_TANIMLARI? odeme_yontemi { get; set; }

    public virtual POLISELER? police { get; set; }

    public virtual KULLANICILAR? tahsilat_yapan_kullanici { get; set; }

    public virtual TAKSITLER? taksit { get; set; }
}
