namespace SigortaYonetimAPI.Models.DTOs
{
    // Genel istatistikler için DTO
    public class GenelIstatistiklerDto
    {
        public int ToplamMusteri { get; set; }
        public int ToplamPolice { get; set; }
        public int ToplamHasar { get; set; }
        public int ToplamOdeme { get; set; }
        
        public int BuAyYeniMusteri { get; set; }
        public int BuAyYeniPolice { get; set; }
        public int BuAyYeniHasar { get; set; }
        public decimal BuAyToplamOdeme { get; set; }
        
        public decimal BuYilToplamPrim { get; set; }
        public decimal BuYilToplamHasarTutari { get; set; }
    }

    // Poliçe türlerine göre dağılım için DTO
    public class PoliceTurDagilimiDto
    {
        public string PoliceTuruAdi { get; set; } = string.Empty;
        public int Adet { get; set; }
        public decimal ToplamPrim { get; set; }
    }

    // Sigorta şirketlerine göre dağılım için DTO
    public class SirketDagilimiDto
    {
        public string SirketAdi { get; set; } = string.Empty;
        public int Adet { get; set; }
        public decimal ToplamPrim { get; set; }
    }

    // Aylık trend için DTO
    public class AylikTrendDto
    {
        public int Ay { get; set; }
        public string AyAdi { get; set; } = string.Empty;
        public int Adet { get; set; }
        public decimal ToplamPrim { get; set; }
    }

    // Hasar durumlarına göre dağılım için DTO
    public class HasarDurumDagilimiDto
    {
        public string DurumAdi { get; set; } = string.Empty;
        public int Adet { get; set; }
        public decimal ToplamTutar { get; set; }
    }

    // Ödeme durumlarına göre dağılım için DTO
    public class OdemeDurumDagilimiDto
    {
        public string DurumAdi { get; set; } = string.Empty;
        public int Adet { get; set; }
        public decimal ToplamTutar { get; set; }
    }

    // En çok hasar bildiren müşteriler için DTO
    public class EnCokHasarMusteriDto
    {
        public string MusteriAdi { get; set; } = string.Empty;
        public int HasarSayisi { get; set; }
        public decimal ToplamTutar { get; set; }
    }

    // En yüksek primli poliçeler için DTO
    public class EnYuksekPrimPoliceDto
    {
        public string PoliceNo { get; set; } = string.Empty;
        public string MusteriAdi { get; set; } = string.Empty;
        public string PoliceTuruAdi { get; set; } = string.Empty;
        public decimal BrutPrim { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

    // Komisyon raporu için DTO
    public class KomisyonRaporuDto
    {
        public decimal ToplamKomisyon { get; set; }
        public decimal ToplamPrim { get; set; }
        public int PoliceSayisi { get; set; }
        public decimal OrtalamaKomisyonOrani { get; set; }
    }
} 