using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class ROLLER
{
    public int id { get; set; }

    public string rol_adi { get; set; } = null!;

    public string? aciklama { get; set; }

    public bool aktif_mi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual ICollection<KULLANICI_ROLLER> KULLANICI_ROLLERs { get; set; } = new List<KULLANICI_ROLLER>();
}
