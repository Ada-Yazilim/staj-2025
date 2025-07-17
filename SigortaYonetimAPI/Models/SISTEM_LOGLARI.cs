using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class SISTEM_LOGLARI
{
    public int id { get; set; }

    public int? kullanici_id { get; set; }

    public int? islem_tipi_id { get; set; }

    public string? tablo_adi { get; set; }

    public int? kayit_id { get; set; }

    public string? eski_deger { get; set; }

    public string? yeni_deger { get; set; }

    public string? ip_adresi { get; set; }

    public string? tarayici_bilgisi { get; set; }

    public DateTime islem_tarihi { get; set; }

    public string? aciklama { get; set; }

    public virtual DURUM_TANIMLARI? islem_tipi { get; set; }

    public virtual KULLANICILAR? kullanici { get; set; }
}
