using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class BILDIRIMLER
{
    public int id { get; set; }

    public int alici_kullanici_id { get; set; }

    public int? bildirim_tipi_id { get; set; }

    public string baslik { get; set; } = null!;

    public string icerik { get; set; } = null!;

    public int? oncelik_id { get; set; }

    public bool? okundu_mu { get; set; }

    public DateTime gonderim_tarihi { get; set; }

    public DateTime? okunma_tarihi { get; set; }

    public int durum_id { get; set; }

    public string? hata_mesaji { get; set; }

    public virtual KULLANICILAR alici_kullanici { get; set; } = null!;

    public virtual DURUM_TANIMLARI? bildirim_tipi { get; set; }

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;

    public virtual DURUM_TANIMLARI? oncelik { get; set; }
}
