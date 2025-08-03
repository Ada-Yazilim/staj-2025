using System.ComponentModel.DataAnnotations;
using SigortaYonetimAPI.Validations;

namespace SigortaYonetimAPI.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalıdır")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Ad sadece harf içerebilir")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [MinLength(2, ErrorMessage = "Soyad en az 2 karakter olmalıdır")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Soyad sadece harf içerebilir")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon alanı zorunludur")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır (5XX XXX XX XX formatında)")]
        public string Telefon { get; set; } = string.Empty;

        // TC Kimlik No (opsiyonel)
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No 11 haneli olmalıdır")]
        public string? TcKimlikNo { get; set; }

        // Doğum tarihi (opsiyonel)
        public DateTime? DogumTarihi { get; set; }

        // Cinsiyet (opsiyonel)
        [Range(1, 2, ErrorMessage = "Geçerli bir cinsiyet seçiniz")]
        public int? Cinsiyet { get; set; } = 1; // Varsayılan: Erkek

        // Medeni durum (opsiyonel)
        [Range(1, 5, ErrorMessage = "Geçerli bir medeni durum seçiniz")]
        public int? MedeniDurum { get; set; } = 1; // Varsayılan: Bekar

        // Meslek (opsiyonel)
        [StringLength(100, ErrorMessage = "Meslek en fazla 100 karakter olabilir")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Meslek sadece harf içerebilir")]
        public string? Meslek { get; set; }

        // Eğitim durumu (opsiyonel)
        [Range(1, 6, ErrorMessage = "Geçerli bir eğitim durumu seçiniz")]
        public int? EgitimDurumu { get; set; } = 1; // Varsayılan: İlkokul

        // Aylık gelir (opsiyonel)
        [Range(0, 1000000, ErrorMessage = "Aylık gelir 0-1.000.000 TL arasında olmalıdır")]
        public decimal? AylikGelir { get; set; }

        // Adres bilgileri (zorunlu)
        [Required(ErrorMessage = "İl alanı zorunludur")]
        [StringLength(50, ErrorMessage = "İl adı en fazla 50 karakter olabilir")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "İl adı sadece harf içerebilir")]
        public string AdresIl { get; set; } = string.Empty;

        [Required(ErrorMessage = "İlçe alanı zorunludur")]
        [StringLength(50, ErrorMessage = "İlçe adı en fazla 50 karakter olabilir")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "İlçe adı sadece harf içerebilir")]
        public string AdresIlce { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mahalle alanı zorunludur")]
        [StringLength(100, ErrorMessage = "Mahalle adı en fazla 100 karakter olabilir")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s0-9]+$", ErrorMessage = "Mahalle adı sadece harf ve rakam içerebilir")]
        public string AdresMahalle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres detayı zorunludur")]
        [StringLength(500, ErrorMessage = "Adres detayı en fazla 500 karakter olabilir")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s0-9.,/-]+$", ErrorMessage = "Adres detayında geçersiz karakter bulunmaktadır")]
        public string AdresDetay { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Posta kodu en fazla 10 karakter olabilir")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Posta kodu sadece rakam içerebilir")]
        public string? PostaKodu { get; set; }
    }
} 