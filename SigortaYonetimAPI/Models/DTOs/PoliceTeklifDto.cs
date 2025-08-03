namespace SigortaYonetimAPI.Models.DTOs
{
    public class PoliceTeklifListDto
    {
        public int id { get; set; }
        public string teklif_no { get; set; } = string.Empty;
        public string musteri_adi { get; set; } = string.Empty;
        public string police_turu_adi { get; set; } = string.Empty;
        public string sigorta_sirketi_adi { get; set; } = string.Empty;
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? toplam_tutar { get; set; }
        public string durum_adi { get; set; } = string.Empty;
        public DateTime teklif_tarihi { get; set; }
        public DateTime? gecerlilik_tarihi { get; set; }
        public string olusturan_kullanici { get; set; } = string.Empty;
    }

    public class PoliceTeklifDetayDto
    {
        public int id { get; set; }
        public string teklif_no { get; set; } = string.Empty;
        public int musteri_id { get; set; }
        public string musteri_adi { get; set; } = string.Empty;
        public int police_turu_id { get; set; }
        public string police_turu_adi { get; set; } = string.Empty;
        public int sigorta_sirketi_id { get; set; }
        public string sigorta_sirketi_adi { get; set; } = string.Empty;
        public int olusturan_kullanici_id { get; set; }
        public string olusturan_kullanici { get; set; } = string.Empty;
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public int durum_id { get; set; }
        public string durum_adi { get; set; } = string.Empty;
        public DateTime teklif_tarihi { get; set; }
        public DateTime? gecerlilik_tarihi { get; set; }
        public DateTime? onay_tarihi { get; set; }
        public string? onaylayan_kullanici { get; set; }
        public string? red_nedeni { get; set; }
        public string? notlar { get; set; }
        public DateTime olusturma_tarihi { get; set; }
        public DateTime guncelleme_tarihi { get; set; }
    }

    public class PoliceTeklifCreateDto
    {
        public int musteri_id { get; set; }
        public int police_turu_id { get; set; }
        public int sigorta_sirketi_id { get; set; }
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public string? notlar { get; set; }
    }

    public class PoliceTeklifUpdateDto
    {
        public int id { get; set; }
        public int musteri_id { get; set; }
        public int police_turu_id { get; set; }
        public int sigorta_sirketi_id { get; set; }
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public string? notlar { get; set; }
    }
} 