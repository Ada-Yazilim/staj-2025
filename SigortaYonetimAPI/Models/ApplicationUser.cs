using Microsoft.AspNetCore.Identity;

namespace SigortaYonetimAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public bool EmailDogrulandi { get; set; }
        public bool TelefonDogrulandi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public int BasarisizGirisSayisi { get; set; } = 0;
        public DateTime? HesapKilitlenmeTarihi { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
        
        // Navigation property to KULLANICILAR table
        public int? KullanicilarId { get; set; }
        public virtual KULLANICILAR? Kullanici { get; set; }
        
        // Additional properties for enhanced user management
        public string? Pozisyon { get; set; }
        public string? Departman { get; set; }
        public string? YoneticiId { get; set; }
        public virtual ApplicationUser? Yonetici { get; set; }
        public virtual ICollection<ApplicationUser> AstKullanicilar { get; set; } = new List<ApplicationUser>();
        
        // Sistem logları ve aktivite takibi
        public DateTime? SonAktiviteTarihi { get; set; }
        public string? SonIpAdresi { get; set; }
        public string? Notlar { get; set; }
        public bool AktifMi { get; set; } = true;
        
        // Calculated Properties
        public string TamAd => $"{Ad} {Soyad}";
        public bool HesapKilitliMi => HesapKilitlenmeTarihi.HasValue && HesapKilitlenmeTarihi > DateTime.Now;
    }
} 