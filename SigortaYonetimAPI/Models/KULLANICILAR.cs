using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class KULLANICILAR
{
    public int id { get; set; }

    public string ad { get; set; } = null!;

    public string soyad { get; set; } = null!;

    public string eposta { get; set; } = null!;

    public string sifre_hash { get; set; } = null!;

    public string? telefon { get; set; }

    public int durum_id { get; set; }

    public bool email_dogrulandi { get; set; }

    public bool telefon_dogrulandi { get; set; }

    public DateTime? son_giris_tarihi { get; set; }

    public int? basarisiz_giris_sayisi { get; set; }

    public DateTime? hesap_kilitlenme_tarihi { get; set; }

    public DateTime kayit_tarihi { get; set; }

    public DateTime guncelleme_tarihi { get; set; }

    public virtual ICollection<BILDIRIMLER> BILDIRIMLERs { get; set; } = new List<BILDIRIMLER>();

    public virtual ICollection<DOGRULAMA_KODLARI> DOGRULAMA_KODLARIs { get; set; } = new List<DOGRULAMA_KODLARI>();

    public virtual ICollection<DOKUMANLAR> DOKUMANLARs { get; set; } = new List<DOKUMANLAR>();

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARbildiren_kullanicis { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARsorumlu_ekspers { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<HASAR_TAKIP_NOTLARI> HASAR_TAKIP_NOTLARIs { get; set; } = new List<HASAR_TAKIP_NOTLARI>();

    public virtual ICollection<KOMISYON_HESAPLARI> KOMISYON_HESAPLARIs { get; set; } = new List<KOMISYON_HESAPLARI>();

    public virtual ICollection<MUSTERILER> MUSTERILERs { get; set; } = new List<MUSTERILER>();

    public virtual ICollection<ODEMELER> ODEMELERs { get; set; } = new List<ODEMELER>();

    public virtual ICollection<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; } = new List<POLICE_TEKLIFLERI>();

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();

    public virtual ICollection<RAPOR_SABLONLARI> RAPOR_SABLONLARIs { get; set; } = new List<RAPOR_SABLONLARI>();

    public virtual ICollection<SIFRE_SIFIRLAMA> SIFRE_SIFIRLAMAs { get; set; } = new List<SIFRE_SIFIRLAMA>();

    public virtual ICollection<SISTEM_LOGLARI> SISTEM_LOGLARIs { get; set; } = new List<SISTEM_LOGLARI>();

    public virtual DURUM_TANIMLARI durum { get; set; } = null!;
}
