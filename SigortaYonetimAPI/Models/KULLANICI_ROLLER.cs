using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class KULLANICI_ROLLER
{
    public int kullanici_id { get; set; }

    public int rol_id { get; set; }

    public DateTime atanma_tarihi { get; set; }

    public DateTime? bitis_tarihi { get; set; }

    public virtual KULLANICILAR kullanici { get; set; } = null!;

    public virtual ROLLER rol { get; set; } = null!;
}
