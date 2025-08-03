using System.ComponentModel.DataAnnotations;

namespace SigortaYonetimAPI.Models.DTOs
{
    public class HasarListDto
    {
        public int id { get; set; }
        public string dosya_no { get; set; } = string.Empty;
        public string police_no { get; set; } = string.Empty;
        public string musteri_adi { get; set; } = string.Empty;
        public string durum_adi { get; set; } = string.Empty;
        public DateTime olusturma_tarihi { get; set; }
        public decimal? toplam_tutar { get; set; }
    }

    public class HasarDetayDto
    {
        public int id { get; set; }
        public string dosya_no { get; set; } = string.Empty;
        public int police_id { get; set; }
        public string police_no { get; set; } = string.Empty;
        public int musteri_id { get; set; }
        public string musteri_adi { get; set; } = string.Empty;
        public int durum_id { get; set; }
        public string durum_adi { get; set; } = string.Empty;
        public string? aciklama { get; set; }
        public decimal? toplam_tutar { get; set; }
        public DateTime olusturma_tarihi { get; set; }
        public DateTime guncelleme_tarihi { get; set; }
    }

    public class HasarCreateDto
    {
        [Required(ErrorMessage = "Poliçe seçimi zorunludur")]
        public int police_id { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur")]
        public int musteri_id { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? aciklama { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Toplam tutar negatif olamaz")]
        public decimal? toplam_tutar { get; set; }
    }

    public class HasarDurumUpdateDto
    {
        public int durum_id { get; set; }
    }
} 