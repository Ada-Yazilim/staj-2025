using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class OTURUM_KAYITLARI
{
    public int id { get; set; }

    public int kullanici_id { get; set; }

    public string oturum_token { get; set; } = null!;

    public string? ip_adresi { get; set; }

    public string? tarayici_bilgisi { get; set; }

    public string? cihaz_bilgisi { get; set; }

    public DateTime baslangic_tarihi { get; set; }

    public DateTime? bitis_tarihi { get; set; }

    public virtual KULLANICILAR kullanici { get; set; } = null!;
}
