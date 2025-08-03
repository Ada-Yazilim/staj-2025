using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SigortaYonetimAPI.Models.DTOs
{
    public class DokumanDto
    {
        public int Id { get; set; }
        public string DosyaAdi { get; set; } = string.Empty;
        public string? OrijinalDosyaAdi { get; set; }
        public string? DosyaYolu { get; set; }
        public int DosyaBoyutu { get; set; }
        public string? MimeType { get; set; }
        public int? KategoriId { get; set; }
        public string? KategoriAdi { get; set; }
        public string? DosyaTuru { get; set; }
        public string? Aciklama { get; set; }
        public DateTime YuklenmeTarihi { get; set; }
        public int? MusteriId { get; set; }
        public string? MusteriAdi { get; set; }
        public int? PoliceId { get; set; }
        public string? PoliceNo { get; set; }
    }

    public class DokumanDetayDto : DokumanDto
    {
        public string? HashDegeri { get; set; }
        public bool? ZorunluMu { get; set; }
        public int? YukleyenKullaniciId { get; set; }
        public string? YukleyenKullanici { get; set; }
        public DateTime? SonErisimTarihi { get; set; }
        public bool? SilinebilirMi { get; set; }
        public int? GizlilikSeviyesiId { get; set; }
        public string? GizlilikSeviyesi { get; set; }
    }

    public class UploadDokumanDto
    {
        [Required(ErrorMessage = "Dosya seçimi zorunludur")]
        public IFormFile? Dosya { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir ilişki ID'si giriniz")]
        public int? IliskiId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir müşteri ID'si giriniz")]
        public int? MusteriId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir poliçe ID'si giriniz")]
        public int? PoliceId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir ilişki tipi ID'si giriniz")]
        public int? IliskiTipiId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori ID'si giriniz")]
        public int? KategoriId { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Aciklama { get; set; }
    }
} 