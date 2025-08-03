using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class HASAR_DOSYALAR
{
    public int id { get; set; }

    public string hasar_no { get; set; } = null!;

    public string? dosya_no { get; set; }

    public int police_id { get; set; }

    public int musteri_id { get; set; }

    public int bildiren_kullanici_id { get; set; }

    public DateTime olay_tarihi { get; set; }

    public string? olay_saati { get; set; }

    public string? olay_yeri_il { get; set; }

    public string? olay_yeri_ilce { get; set; }

    public string? olay_yeri_detay { get; set; }

    public int? olay_tipi_id { get; set; }

    public string? olay_aciklamasi { get; set; }

    public string? polis_rapor_no { get; set; }

    public bool? polis_raporu_var_mi { get; set; }

    public string? tanik_bilgileri { get; set; }

    public int durum_id { get; set; }

    public decimal? talep_edilen_tutar { get; set; }

    public decimal? ekspertiz_tutari { get; set; }

    public decimal? onaylanan_tutar { get; set; }

    public decimal? odenen_tutar { get; set; }

    public decimal? toplam_tutar { get; set; }

    public string? red_nedeni { get; set; }

    public int? sorumlu_eksper_id { get; set; }

    public DateTime? ekspertiz_tarihi { get; set; }

    public DateTime? karar_tarihi { get; set; }

    public DateTime? odeme_tarihi { get; set; }

    public string? onaylayan_kullanici { get; set; }

    public DateTime bildirim_tarihi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public string? notlar { get; set; }

    public string? aciklama { get; set; }

    public virtual ICollection<HASAR_TAKIP_NOTLARI> HASAR_TAKIP_NOTLARIs { get; set; } = new List<HASAR_TAKIP_NOTLARI>();

    public virtual KULLANICILAR bildiren_kullanici { get; set; } = null!;

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual MUSTERILER musteri { get; set; } = null!;

    public virtual DURUM_TANIMLARI? olay_tipi { get; set; }

    public virtual POLISELER police { get; set; } = null!;

    public virtual KULLANICILAR? sorumlu_eksper { get; set; }
}
