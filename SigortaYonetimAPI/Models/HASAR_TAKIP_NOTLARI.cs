using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class HASAR_TAKIP_NOTLARI
{
    public int id { get; set; }

    public int hasar_id { get; set; }

    public int kullanici_id { get; set; }

    public string not_metni { get; set; } = null!;

    public int? not_tipi_id { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual HASAR_DOSYALAR hasar { get; set; } = null!;

    public virtual KULLANICILAR kullanici { get; set; } = null!;

    public virtual DURUM_TANIMLARI? not_tipi { get; set; }
}
