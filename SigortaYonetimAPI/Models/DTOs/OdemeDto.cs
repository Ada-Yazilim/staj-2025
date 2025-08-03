using System.ComponentModel.DataAnnotations;

namespace SigortaYonetimAPI.Models.DTOs
{
    public class OdemeDto
    {
        public int Id { get; set; }
        public string? OdemeNo { get; set; }
        public int? PoliceId { get; set; }
        public string? PoliceNo { get; set; }
        public int MusteriId { get; set; }
        public string? MusteriAdi { get; set; }
        public string? OdemeTuru { get; set; }
        public decimal Tutar { get; set; }
        public int DurumId { get; set; }
        public string? DurumAdi { get; set; }
        public DateTime OdemeTarihi { get; set; }
        public DateTime? VadeTarihi { get; set; }
        public string? Aciklama { get; set; }
    }

    public class OdemeDetayDto : OdemeDto
    {
        public int? TaksitSayisi { get; set; }
        public decimal? TaksitTutari { get; set; }
        public List<TaksitDto> Taksitler { get; set; } = new List<TaksitDto>();
    }

    public class CreateOdemeDto
    {
        [Required(ErrorMessage = "Poliçe seçimi zorunludur")]
        public int PoliceId { get; set; }

        [Required(ErrorMessage = "Ödeme türü zorunludur")]
        [StringLength(50, ErrorMessage = "Ödeme türü en fazla 50 karakter olabilir")]
        public string OdemeTuru { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tutar zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Tutar { get; set; }

        [Required(ErrorMessage = "Vade tarihi zorunludur")]
        public DateTime VadeTarihi { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Aciklama { get; set; }

        [Range(1, 60, ErrorMessage = "Taksit sayısı 1-60 arasında olmalıdır")]
        public int TaksitSayisi { get; set; } = 1;
    }

    public class UpdateOdemeDurumDto
    {
        public int DurumId { get; set; }
    }

    public class TaksitOdemeDto
    {
        public int TaksitId { get; set; }
        
        [Required(ErrorMessage = "Kart numarası zorunludur")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalıdır")]
        public string KartNo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Son kullanma tarihi zorunludur")]
        public string SonKullanmaTarihi { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CVV zorunludur")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalıdır")]
        public string Cvv { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kart sahibi zorunludur")]
        [StringLength(100, ErrorMessage = "Kart sahibi en fazla 100 karakter olabilir")]
        public string KartSahibi { get; set; } = string.Empty;
    }

    public class TaksitDto
    {
        public int Id { get; set; }
        public int TaksitNo { get; set; }
        public decimal Tutar { get; set; }
        public DateTime VadeTarihi { get; set; }
        public DateTime? OdemeTarihi { get; set; }
        public string? DurumAdi { get; set; }
    }
} 