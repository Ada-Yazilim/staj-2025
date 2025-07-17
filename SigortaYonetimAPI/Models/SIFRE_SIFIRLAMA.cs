using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class SIFRE_SIFIRLAMA
{
    public int id { get; set; }

    public int kullanici_id { get; set; }

    public string token { get; set; } = null!;

    public DateTime son_kullanma_tarihi { get; set; }

    public bool kullanildi_mi { get; set; }

    public string? ip_adresi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual KULLANICILAR kullanici { get; set; } = null!;
}
