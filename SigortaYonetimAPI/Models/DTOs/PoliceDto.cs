namespace SigortaYonetimAPI.Models.DTOs
{
    public class PoliceListDto
    {
        public int id { get; set; }
        public string police_no { get; set; } = string.Empty;
        public string musteri_adi { get; set; } = string.Empty;
        public string police_turu_adi { get; set; } = string.Empty;
        public string sigorta_sirketi_adi { get; set; } = string.Empty;
        public DateTime baslangic_tarihi { get; set; }
        public DateTime bitis_tarihi { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? toplam_tutar { get; set; }
        public string durum_adi { get; set; } = string.Empty;
        public DateTime tanzim_tarihi { get; set; }
        public string tanzim_eden_kullanici { get; set; } = string.Empty;
        public string? teklif_no { get; set; }
    }

    public class PoliceDetayDto
    {
        public int id { get; set; }
        public string police_no { get; set; } = string.Empty;
        public int? teklif_id { get; set; }
        public string? teklif_no { get; set; }
        public int musteri_id { get; set; }
        public string musteri_adi { get; set; } = string.Empty;
        public int police_turu_id { get; set; }
        public string police_turu_adi { get; set; } = string.Empty;
        public int sigorta_sirketi_id { get; set; }
        public string sigorta_sirketi_adi { get; set; } = string.Empty;
        public int tanzim_eden_kullanici_id { get; set; }
        public string tanzim_eden_kullanici { get; set; } = string.Empty;
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public DateTime baslangic_tarihi { get; set; }
        public DateTime bitis_tarihi { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public int? taksit_sayisi { get; set; }
        public int durum_id { get; set; }
        public string durum_adi { get; set; } = string.Empty;
        public string? iptal_nedeni { get; set; }
        public DateTime? iptal_tarihi { get; set; }
        public DateTime? yenileme_hatirlatma_tarihi { get; set; }
        public string? ozel_sartlar { get; set; }
        public string? notlar { get; set; }
        public DateTime tanzim_tarihi { get; set; }
        public DateTime guncelleme_tarihi { get; set; }
    }

    public class PoliceCreateDto
    {
        public int? teklif_id { get; set; }
        public int musteri_id { get; set; }
        public int police_turu_id { get; set; }
        public int sigorta_sirketi_id { get; set; }
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public DateTime baslangic_tarihi { get; set; }
        public DateTime bitis_tarihi { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public int? taksit_sayisi { get; set; }
        public string? ozel_sartlar { get; set; }
        public string? notlar { get; set; }
    }

    public class PoliceUpdateDto
    {
        public int id { get; set; }
        public int? teklif_id { get; set; }
        public int musteri_id { get; set; }
        public int police_turu_id { get; set; }
        public int sigorta_sirketi_id { get; set; }
        public string? risk_bilgileri { get; set; }
        public string? teminat_bilgileri { get; set; }
        public DateTime baslangic_tarihi { get; set; }
        public DateTime bitis_tarihi { get; set; }
        public decimal? brut_prim { get; set; }
        public decimal? net_prim { get; set; }
        public decimal? komisyon_tutari { get; set; }
        public decimal? vergi_tutari { get; set; }
        public decimal? toplam_tutar { get; set; }
        public int? taksit_sayisi { get; set; }
        public string? ozel_sartlar { get; set; }
        public string? notlar { get; set; }
    }
} 