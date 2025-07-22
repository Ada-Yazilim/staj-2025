using Microsoft.AspNetCore.Identity;

namespace SigortaYonetimAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Aciklama { get; set; }
        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
} 