using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class RAPOR_SABLONLARI
{
    public int id { get; set; }

    public string rapor_adi { get; set; } = null!;

    public int? rapor_tipi_id { get; set; }

    public string? sql_sorgusu { get; set; }

    public string? parametreler { get; set; }

    public int? cikti_formati_id { get; set; }

    public bool? otomatik_mi { get; set; }

    public int? zamanlama_id { get; set; }

    public bool aktif_mi { get; set; }

    public int? olusturan_kullanici_id { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual DURUM_TANIMLARI? cikti_formati { get; set; }

    public virtual KULLANICILAR? olusturan_kullanici { get; set; }

    public virtual DURUM_TANIMLARI? rapor_tipi { get; set; }

    public virtual DURUM_TANIMLARI? zamanlama { get; set; }
}
