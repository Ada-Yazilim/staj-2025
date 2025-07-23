namespace SigortaYonetimAPI.Models.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserInfoDto? User { get; set; }
    }

    public class UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        
        // KULLANICILAR tablosu ile entegrasyon
        public int? KullanicilarId { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public bool EmailDogrulandi { get; set; }
    }
} 