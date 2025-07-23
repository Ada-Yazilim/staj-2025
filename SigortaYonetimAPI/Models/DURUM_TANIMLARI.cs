using System;
using System.Collections.Generic;

namespace SigortaYonetimAPI.Models;

public partial class DURUM_TANIMLARI
{
    public int id { get; set; }

    public string tablo_adi { get; set; } = null!;

    public string alan_adi { get; set; } = null!;

    public string deger_kodu { get; set; } = null!;

    public string deger_aciklama { get; set; } = null!;

    public int siralama { get; set; }

    public bool aktif_mi { get; set; }

    public DateTime olusturma_tarihi { get; set; }

    public virtual ICollection<BILDIRIMLER> BILDIRIMLERbildirim_tipis { get; set; } = new List<BILDIRIMLER>();

    public virtual ICollection<BILDIRIMLER> BILDIRIMLERdurums { get; set; } = new List<BILDIRIMLER>();

    public virtual ICollection<BILDIRIMLER> BILDIRIMLERonceliks { get; set; } = new List<BILDIRIMLER>();

    public virtual ICollection<DOGRULAMA_KODLARI> DOGRULAMA_KODLARIamacs { get; set; } = new List<DOGRULAMA_KODLARI>();

    public virtual ICollection<DOGRULAMA_KODLARI> DOGRULAMA_KODLARItips { get; set; } = new List<DOGRULAMA_KODLARI>();

    public virtual ICollection<DOKUMANLAR> DOKUMANLARdosya_tipis { get; set; } = new List<DOKUMANLAR>();

    public virtual ICollection<DOKUMANLAR> DOKUMANLARgizlilik_seviyesis { get; set; } = new List<DOKUMANLAR>();

    public virtual ICollection<DOKUMANLAR> DOKUMANLARiliski_tipis { get; set; } = new List<DOKUMANLAR>();

    public virtual ICollection<DOKUMANLAR> DOKUMANLARkategoris { get; set; } = new List<DOKUMANLAR>();

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARdurums { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<HASAR_DOSYALAR> HASAR_DOSYALARolay_tipis { get; set; } = new List<HASAR_DOSYALAR>();

    public virtual ICollection<HASAR_TAKIP_NOTLARI> HASAR_TAKIP_NOTLARIs { get; set; } = new List<HASAR_TAKIP_NOTLARI>();

    public virtual ICollection<KOMISYON_HESAPLARI> KOMISYON_HESAPLARIs { get; set; } = new List<KOMISYON_HESAPLARI>();

    public virtual ICollection<KULLANICILAR> KULLANICILARs { get; set; } = new List<KULLANICILAR>();

    public virtual ICollection<MUSTERILER> MUSTERILERcinsiyets { get; set; } = new List<MUSTERILER>();

    public virtual ICollection<MUSTERILER> MUSTERILERegitim_durumus { get; set; } = new List<MUSTERILER>();

    public virtual ICollection<MUSTERILER> MUSTERILERmedeni_durums { get; set; } = new List<MUSTERILER>();

    public virtual ICollection<MUSTERILER> MUSTERILERtips { get; set; } = new List<MUSTERILER>();

    public virtual ICollection<ODEMELER> ODEMELERdurums { get; set; } = new List<ODEMELER>();

    public virtual ICollection<ODEMELER> ODEMELERodeme_tipis { get; set; } = new List<ODEMELER>();

    public virtual ICollection<ODEMELER> ODEMELERodeme_yontemis { get; set; } = new List<ODEMELER>();

    public virtual ICollection<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; } = new List<POLICE_TEKLIFLERI>();

    public virtual ICollection<POLICE_TURLERI> POLICE_TURLERIalt_kategoris { get; set; } = new List<POLICE_TURLERI>();

    public virtual ICollection<POLICE_TURLERI> POLICE_TURLERIkategoris { get; set; } = new List<POLICE_TURLERI>();

    public virtual ICollection<POLISELER> POLISELERs { get; set; } = new List<POLISELER>();

    public virtual ICollection<RAPOR_SABLONLARI> RAPOR_SABLONLARIcikti_formatis { get; set; } = new List<RAPOR_SABLONLARI>();

    public virtual ICollection<RAPOR_SABLONLARI> RAPOR_SABLONLARIrapor_tipis { get; set; } = new List<RAPOR_SABLONLARI>();

    public virtual ICollection<RAPOR_SABLONLARI> RAPOR_SABLONLARIzamanlamas { get; set; } = new List<RAPOR_SABLONLARI>();

    public virtual ICollection<SISTEM_LOGLARI> SISTEM_LOGLARIs { get; set; } = new List<SISTEM_LOGLARI>();

    public virtual ICollection<TAKSITLER> TAKSITLERdurums { get; set; } = new List<TAKSITLER>();

    public virtual ICollection<TAKSITLER> TAKSITLERhatirlatma_durumus { get; set; } = new List<TAKSITLER>();
}
