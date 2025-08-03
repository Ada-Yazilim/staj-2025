using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class DOKUMANLAR
{
    public int id { get; set; }

    public int? iliski_id { get; set; }

    public int? musteri_id { get; set; }

    public int? police_id { get; set; }

    public int? iliski_tipi_id { get; set; }

    public string dosya_adi { get; set; } = null!;

    public string? orijinal_dosya_adi { get; set; }

    public string? dosya_yolu { get; set; }

    public int? dosya_tipi_id { get; set; }

    public int? dosya_boyutu { get; set; }

    public long? dosya_boyutu_long { get; set; }

    public string? mime_type { get; set; }

    public string? hash_degeri { get; set; }

    public int? kategori_id { get; set; }

    public string? dosya_turu { get; set; }

    public string? aciklama { get; set; }

    public bool? zorunlu_mu { get; set; }

    public int? yukleyen_kullanici_id { get; set; }

    public DateTime yuklenme_tarihi { get; set; }

    public DateTime yukleme_tarihi { get; set; }

    public DateTime? son_erisim_tarihi { get; set; }

    public bool? silinebilir_mi { get; set; }

    public int? gizlilik_seviyesi_id { get; set; }

    public virtual DURUM_TANIMLARI? dosya_tipi { get; set; }

    public virtual DURUM_TANIMLARI? gizlilik_seviyesi { get; set; }

    public virtual DURUM_TANIMLARI? iliski_tipi { get; set; }

    public virtual DURUM_TANIMLARI? kategori { get; set; }

    public virtual KULLANICILAR? yukleyen_kullanici { get; set; }

    public virtual MUSTERILER? musteri { get; set; }

    public virtual POLISELER? police { get; set; }
}
