using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class MUSTERILER
{
    public int id { get; set; }

    public int? kullanici_id { get; set; }

    public string musteri_no { get; set; } = null!;

    public int tip_id { get; set; }

    public string? ad { get; set; }

    public string? soyad { get; set; }

    public string? sirket_adi { get; set; }

    public string? vergi_no { get; set; }

    public string? tc_kimlik_no { get; set; }

    public string? eposta { get; set; }

    public string? telefon { get; set; }

    public string? cep_telefonu { get; set; }

    public DateOnly? dogum_tarihi { get; set; }

    public int? cinsiyet_id { get; set; }

    public int? medeni_durum_id { get; set; }

    public string? meslek { get; set; }

    public int? egitim_durumu_id { get; set; }

    public decimal? aylik_gelir { get; set; }

    public string? adres_il { get; set; }

    public string? adres_ilce { get; set; }

    public string? adres_mahalle { get; set; }

    public string? adres_detay { get; set; }

    public string? posta_kodu { get; set; }

    public string? not_bilgileri { get; set; }

    public bool? blacklist_mi { get; set; }

    public string? blacklist_nedeni { get; set; }

    public DateTime kayit_tarihi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public string? kaydeden_kullanici { get; set; }

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARs { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<MUSTERI_ILETISIM_TERCIHLERI> MUSTERI_ILETISIM_TERCIHLERIs { get; set; } = new List<MUSTERI_ILETISIM_TERCIHLERI>();

    public virtual ICollection<ODEMELER> ODEMELERs { get; set; } = new List<ODEMELER>();

    public virtual ICollection<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; } = new List<POLICE_TEKLIFLERI>();

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();

    public virtual DURUM_TANIMLARI? cinsiyet { get; set; }

    public virtual DURUM_TANIMLARI? egitim_durumu { get; set; }

    public virtual KULLANICILAR? kullanici { get; set; }

    public virtual DURUM_TANIMLARI? medeni_durum { get; set; }

    public virtual DURUM_TANIMLARI tip { get; set; } = null!;
    
    // Identity Framework entegrasyonu
    public string? ApplicationUserId { get; set; }
    public virtual ApplicationUser? ApplicationUser { get; set; }
}
