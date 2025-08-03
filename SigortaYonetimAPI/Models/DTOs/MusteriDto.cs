using System.ComponentModel.DataAnnotations;
using SigortaYonetimAPI.Validations;

namespace SigortaYonetimAPI.Models.DTOs
{
    public class MusteriListDto
    {
        public int id { get; set; }
        public string musteri_no { get; set; } = string.Empty;
        public string tip_adi { get; set; } = string.Empty; // DURUM_TANIMLARI'ndan
        public string? ad { get; set; }
        public string? soyad { get; set; }
        public string? sirket_adi { get; set; }
        public string? eposta { get; set; }
        public string? telefon { get; set; }
        public string? adres_il { get; set; }
        public bool? blacklist_mi { get; set; }
        public DateTime kayit_tarihi { get; set; }
        public string tam_ad => !string.IsNullOrEmpty(sirket_adi) ? sirket_adi : $"{ad} {soyad}";
    }

    public class MusteriDetayDto
    {
        public int id { get; set; }
        public string musteri_no { get; set; } = string.Empty;
        public string? ad { get; set; }
        public string? soyad { get; set; }
        public string? sirket_adi { get; set; }
        public string? vergi_no { get; set; }
        public string? tc_kimlik_no { get; set; }
        public string? eposta { get; set; }
        public string? telefon { get; set; }
        public DateOnly? dogum_tarihi { get; set; }
        public int? cinsiyet_id { get; set; }
        public string? cinsiyet_adi { get; set; }
        public int? medeni_durum_id { get; set; }
        public string? medeni_durum_adi { get; set; }
        public string? meslek { get; set; }
        public int? egitim_durumu_id { get; set; }
        public string? egitim_durumu_adi { get; set; }
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
        
        // İstatistikler
        public int toplam_police_sayisi { get; set; }
        public decimal toplam_prim_tutari { get; set; }
        public int aktif_police_sayisi { get; set; }
        public int hasar_sayisi { get; set; }
    }

    public class MusteriCreateDto
    {
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string? ad { get; set; }

        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string? soyad { get; set; }

        [StringLength(100, ErrorMessage = "Şirket adı en fazla 100 karakter olabilir")]
        public string? sirket_adi { get; set; }

        [TurkishVergiNo(ErrorMessage = "Geçerli bir vergi numarası giriniz (10 haneli)")]
        [StringLength(20, ErrorMessage = "Vergi numarası en fazla 20 karakter olabilir")]
        public string? vergi_no { get; set; }

        [TurkishTcKimlik(ErrorMessage = "Geçerli bir TC Kimlik numarası giriniz (11 haneli)")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 karakter olmalıdır")]
        public string? tc_kimlik_no { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string? eposta { get; set; }

        [TurkishPhone(ErrorMessage = "Geçerli bir Türkiye telefon numarası giriniz (Örn: 0212 123 45 67)")]
        [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
        public string? telefon { get; set; }

        public DateOnly? dogum_tarihi { get; set; }
        public int? cinsiyet_id { get; set; }
        public int? medeni_durum_id { get; set; }

        [StringLength(50, ErrorMessage = "Meslek en fazla 50 karakter olabilir")]
        public string? meslek { get; set; }

        public int? egitim_durumu_id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Aylık gelir negatif olamaz")]
        public decimal? aylik_gelir { get; set; }

        [StringLength(50, ErrorMessage = "İl en fazla 50 karakter olabilir")]
        public string? adres_il { get; set; }

        [StringLength(50, ErrorMessage = "İlçe en fazla 50 karakter olabilir")]
        public string? adres_ilce { get; set; }

        [StringLength(50, ErrorMessage = "Mahalle en fazla 50 karakter olabilir")]
        public string? adres_mahalle { get; set; }

        [StringLength(255, ErrorMessage = "Adres detayı en fazla 255 karakter olabilir")]
        public string? adres_detay { get; set; }

        [StringLength(10, ErrorMessage = "Posta kodu en fazla 10 karakter olabilir")]
        public string? posta_kodu { get; set; }

        [StringLength(255, ErrorMessage = "Not bilgileri en fazla 255 karakter olabilir")]
        public string? not_bilgileri { get; set; }

        public bool? blacklist_mi { get; set; } = false;

        [StringLength(255, ErrorMessage = "Blacklist nedeni en fazla 255 karakter olabilir")]
        public string? blacklist_nedeni { get; set; }
    }

    public class MusteriUpdateDto : MusteriCreateDto
    {
        [Required]
        public int id { get; set; }
    }

    public class MusteriSearchDto
    {
        public string? arama_metni { get; set; } // Genel arama
        public string? musteri_no { get; set; }
        public string? tc_kimlik_no { get; set; }
        public string? vergi_no { get; set; }
        public string? eposta { get; set; }
        public string? telefon { get; set; }
        public string? adres_il { get; set; }
        public bool? blacklist_mi { get; set; }
        public DateTime? kayit_tarihi_baslangic { get; set; }
        public DateTime? kayit_tarihi_bitis { get; set; }
        public int sayfa { get; set; } = 1;
        public int sayfa_boyutu { get; set; } = 20;
        public string siralama { get; set; } = "kayit_tarihi_desc"; // kayit_tarihi_desc, ad_asc, vs.
    }

    public class MusteriIstatistikDto
    {
        public int toplam_musteri_sayisi { get; set; }
        public int bireysel_musteri_sayisi { get; set; }
        public int kurumsal_musteri_sayisi { get; set; }
        public int blacklist_musteri_sayisi { get; set; }
        public int bu_ay_eklenen_sayisi { get; set; }
        public decimal ortalama_aylık_gelir { get; set; }
        public List<IlBazindaMusteriDto> il_bazinda_dagilim { get; set; } = new();
    }

    public class IlBazindaMusteriDto
    {
        public string il_adi { get; set; } = string.Empty;
        public int musteri_sayisi { get; set; }
    }
} 