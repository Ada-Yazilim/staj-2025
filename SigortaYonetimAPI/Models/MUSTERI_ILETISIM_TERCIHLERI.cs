using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class MUSTERI_ILETISIM_TERCIHLERI
{
    public int id { get; set; }

    public int musteri_id { get; set; }

    public bool email_bildirimi { get; set; }

    public bool sms_bildirimi { get; set; }

    public bool whatsapp_bildirimi { get; set; }

    public bool arama_bildirimi { get; set; }

    public bool pazarlama_onayi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public virtual MUSTERILER musteri { get; set; } = null!;
}
