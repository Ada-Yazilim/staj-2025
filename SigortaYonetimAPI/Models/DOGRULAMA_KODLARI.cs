using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class DOGRULAMA_KODLARI
{
    public int id { get; set; }

    public int kullanici_id { get; set; }

    public string kod { get; set; } = null!;

    public int tip_id { get; set; }

    public int amac_id { get; set; }

    public DateTime son_kullanma_tarihi { get; set; }

    public bool kullanildi_mi { get; set; }

    public int? deneme_sayisi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual DURUM_TANIMLARI amac { get; set; } = null!;

    public virtual KULLANICILAR kullanici { get; set; } = null!;

    public virtual DURUM_TANIMLARI tip { get; set; } = null!;
}
