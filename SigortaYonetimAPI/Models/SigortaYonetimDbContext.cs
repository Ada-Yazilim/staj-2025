using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SigortaYonetimAPI.Models;

public partial class SigortaYonetimDbContext : DbContext
{
    public SigortaYonetimDbContext()
    {
    }

    public SigortaYonetimDbContext(DbContextOptions<SigortaYonetimDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BILDIRIMLER> BILDIRIMLERs { get; set; }

    public virtual DbSet<DOGRULAMA_KODLARI> DOGRULAMA_KODLARIs { get; set; }

    public virtual DbSet<DOKUMANLAR> DOKUMANLARs { get; set; }

    public virtual DbSet<DURUM_TANIMLARI> DURUM_TANIMLARIs { get; set; }

    public virtual DbSet<HASAR_DOSYALAR> HASAR_DOSYALARs { get; set; }

    public virtual DbSet<HASAR_TAKIP_NOTLARI> HASAR_TAKIP_NOTLARIs { get; set; }

    public virtual DbSet<KOMISYON_HESAPLARI> KOMISYON_HESAPLARIs { get; set; }

    public virtual DbSet<KULLANICILAR> KULLANICILARs { get; set; }

    public virtual DbSet<KULLANICI_ROLLER> KULLANICI_ROLLERs { get; set; }

    public virtual DbSet<MUSTERILER> MUSTERILERs { get; set; }

    public virtual DbSet<MUSTERI_ILETISIM_TERCIHLERI> MUSTERI_ILETISIM_TERCIHLERIs { get; set; }

    public virtual DbSet<ODEMELER> ODEMELERs { get; set; }

    public virtual DbSet<OTURUM_KAYITLARI> OTURUM_KAYITLARIs { get; set; }

    public virtual DbSet<POLICE_TEKLIFLERI> POLICE_TEKLIFLERIs { get; set; }

    public virtual DbSet<POLICE_TURLERI> POLICE_TURLERIs { get; set; }

    public virtual DbSet<POLISELER> POLISELERs { get; set; }

    public virtual DbSet<RAPOR_SABLONLARI> RAPOR_SABLONLARIs { get; set; }

    public virtual DbSet<ROLLER> ROLLERs { get; set; }

    public virtual DbSet<SIFRE_SIFIRLAMA> SIFRE_SIFIRLAMAs { get; set; }

    public virtual DbSet<SIGORTA_SIRKETLERI> SIGORTA_SIRKETLERIs { get; set; }

    public virtual DbSet<SISTEM_LOGLARI> SISTEM_LOGLARIs { get; set; }

    public virtual DbSet<TAKSITLER> TAKSITLERs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=SigortaYonetimDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BILDIRIMLER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__BILDIRIM__3213E83F5EDBC28A");

            entity.ToTable("BILDIRIMLER");

            entity.Property(e => e.baslik).HasMaxLength(100);
            entity.Property(e => e.gonderim_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.hata_mesaji).HasMaxLength(255);
            entity.Property(e => e.icerik).HasMaxLength(255);
            entity.Property(e => e.okundu_mu).HasDefaultValue(false);
            entity.Property(e => e.okunma_tarihi).HasColumnType("datetime");

            entity.HasOne(d => d.alici_kullanici).WithMany(p => p.BILDIRIMLERs)
                .HasForeignKey(d => d.alici_kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BILDIRIML__alici__489AC854");

            entity.HasOne(d => d.bildirim_tipi).WithMany(p => p.BILDIRIMLERbildirim_tipis)
                .HasForeignKey(d => d.bildirim_tipi_id)
                .HasConstraintName("FK__BILDIRIML__bildi__498EEC8D");

            entity.HasOne(d => d.durum).WithMany(p => p.BILDIRIMLERdurums)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BILDIRIML__durum__4B7734FF");

            entity.HasOne(d => d.oncelik).WithMany(p => p.BILDIRIMLERonceliks)
                .HasForeignKey(d => d.oncelik_id)
                .HasConstraintName("FK__BILDIRIML__oncel__4A8310C6");
        });

        modelBuilder.Entity<DOGRULAMA_KODLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__DOGRULAM__3213E83FC3C872D7");

            entity.ToTable("DOGRULAMA_KODLARI");

            entity.Property(e => e.deneme_sayisi).HasDefaultValue(0);
            entity.Property(e => e.kod).HasMaxLength(20);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.son_kullanma_tarihi).HasColumnType("datetime");

            entity.HasOne(d => d.amac).WithMany(p => p.DOGRULAMA_KODLARIamacs)
                .HasForeignKey(d => d.amac_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOGRULAMA__amac___4D94879B");

            entity.HasOne(d => d.kullanici).WithMany(p => p.DOGRULAMA_KODLARIs)
                .HasForeignKey(d => d.kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOGRULAMA__kulla__4BAC3F29");

            entity.HasOne(d => d.tip).WithMany(p => p.DOGRULAMA_KODLARItips)
                .HasForeignKey(d => d.tip_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOGRULAMA__tip_i__4CA06362");
        });

        modelBuilder.Entity<DOKUMANLAR>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__DOKUMANL__3213E83F60EA8DE3");

            entity.ToTable("DOKUMANLAR");

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.dosya_adi).HasMaxLength(255);
            entity.Property(e => e.dosya_yolu).HasMaxLength(255);
            entity.Property(e => e.hash_degeri).HasMaxLength(255);
            entity.Property(e => e.mime_type).HasMaxLength(100);
            entity.Property(e => e.orijinal_dosya_adi).HasMaxLength(255);
            entity.Property(e => e.silinebilir_mi).HasDefaultValue(true);
            entity.Property(e => e.son_erisim_tarihi).HasColumnType("datetime");
            entity.Property(e => e.yuklenme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.zorunlu_mu).HasDefaultValue(false);

            entity.HasOne(d => d.dosya_tipi).WithMany(p => p.DOKUMANLARdosya_tipis)
                .HasForeignKey(d => d.dosya_tipi_id)
                .HasConstraintName("FK__DOKUMANLA__dosya__3C34F16F");

            entity.HasOne(d => d.gizlilik_seviyesi).WithMany(p => p.DOKUMANLARgizlilik_seviyesis)
                .HasForeignKey(d => d.gizlilik_seviyesi_id)
                .HasConstraintName("FK__DOKUMANLA__gizli__3E1D39E1");

            entity.HasOne(d => d.iliski_tipi).WithMany(p => p.DOKUMANLARiliski_tipis)
                .HasForeignKey(d => d.iliski_tipi_id)
                .HasConstraintName("FK__DOKUMANLA__ilisk__3B40CD36");

            entity.HasOne(d => d.kategori).WithMany(p => p.DOKUMANLARkategoris)
                .HasForeignKey(d => d.kategori_id)
                .HasConstraintName("FK__DOKUMANLA__kateg__3D2915A8");

            entity.HasOne(d => d.yukleyen_kullanici).WithMany(p => p.DOKUMANLARs)
                .HasForeignKey(d => d.yukleyen_kullanici_id)
                .HasConstraintName("FK__DOKUMANLA__yukle__3F115E1A");
        });

        modelBuilder.Entity<DURUM_TANIMLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__DURUM_TA__3213E83F7B05CF3F");

            entity.ToTable("DURUM_TANIMLARI");

            entity.Property(e => e.aktif_mi).HasDefaultValue(true);
            entity.Property(e => e.alan_adi).HasMaxLength(50);
            entity.Property(e => e.deger_aciklama).HasMaxLength(255);
            entity.Property(e => e.deger_kodu).HasMaxLength(50);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.tablo_adi).HasMaxLength(50);
        });

        modelBuilder.Entity<HASAR_DOSYALAR>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__HASAR_DO__3213E83F9FD228F4");

            entity.ToTable("HASAR_DOSYALAR");

            entity.HasIndex(e => e.hasar_no, "UQ__HASAR_DO__59C8A1EAB87CF833").IsUnique();

            entity.Property(e => e.bildirim_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ekspertiz_tarihi).HasColumnType("datetime");
            entity.Property(e => e.ekspertiz_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.hasar_no).HasMaxLength(30);
            entity.Property(e => e.karar_tarihi).HasColumnType("datetime");
            entity.Property(e => e.notlar).HasMaxLength(255);
            entity.Property(e => e.odeme_tarihi).HasColumnType("datetime");
            entity.Property(e => e.odenen_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.olay_aciklamasi).HasMaxLength(255);
            entity.Property(e => e.olay_saati).HasMaxLength(10);
            entity.Property(e => e.olay_tarihi).HasColumnType("datetime");
            entity.Property(e => e.olay_yeri_detay).HasMaxLength(255);
            entity.Property(e => e.olay_yeri_il).HasMaxLength(50);
            entity.Property(e => e.olay_yeri_ilce).HasMaxLength(50);
            entity.Property(e => e.onaylanan_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.onaylayan_kullanici).HasMaxLength(50);
            entity.Property(e => e.polis_rapor_no).HasMaxLength(50);
            entity.Property(e => e.polis_raporu_var_mi).HasDefaultValue(false);
            entity.Property(e => e.red_nedeni).HasMaxLength(255);
            entity.Property(e => e.talep_edilen_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.tanik_bilgileri).HasMaxLength(255);

            entity.HasOne(d => d.bildiren_kullanici).WithMany(p => p.HASAR_DOSYALARbildiren_kullanicis)
                .HasForeignKey(d => d.bildiren_kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_DOS__bildi__17036CC0");

            entity.HasOne(d => d.durum).WithMany(p => p.HASAR_DOSYALARdurums)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_DOS__durum__18EBB532");

            entity.HasOne(d => d.musteri).WithMany(p => p.HASAR_DOSYALARs)
                .HasForeignKey(d => d.musteri_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_DOS__muste__160F4887");

            entity.HasOne(d => d.olay_tipi).WithMany(p => p.HASAR_DOSYALARolay_tipis)
                .HasForeignKey(d => d.olay_tipi_id)
                .HasConstraintName("FK__HASAR_DOS__olay___17F790F9");

            entity.HasOne(d => d.police).WithMany(p => p.HASAR_DOSYALARs)
                .HasForeignKey(d => d.police_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_DOS__polic__151B244E");

            entity.HasOne(d => d.sorumlu_eksper).WithMany(p => p.HASAR_DOSYALARsorumlu_ekspers)
                .HasForeignKey(d => d.sorumlu_eksper_id)
                .HasConstraintName("FK__HASAR_DOS__sorum__19DFD96B");
        });

        modelBuilder.Entity<HASAR_TAKIP_NOTLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__HASAR_TA__3213E83F95ED626A");

            entity.ToTable("HASAR_TAKIP_NOTLARI");

            entity.Property(e => e.not_metni).HasMaxLength(255);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.hasar).WithMany(p => p.HASAR_TAKIP_NOTLARIs)
                .HasForeignKey(d => d.hasar_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_TAK__hasar__1DB06A4F");

            entity.HasOne(d => d.kullanici).WithMany(p => p.HASAR_TAKIP_NOTLARIs)
                .HasForeignKey(d => d.kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HASAR_TAK__kulla__1EA48E88");

            entity.HasOne(d => d.not_tipi).WithMany(p => p.HASAR_TAKIP_NOTLARIs)
                .HasForeignKey(d => d.not_tipi_id)
                .HasConstraintName("FK__HASAR_TAK__not_t__1F98B2C1");
        });

        modelBuilder.Entity<KOMISYON_HESAPLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__KOMISYON__3213E83F1FB37110");

            entity.ToTable("KOMISYON_HESAPLARI");

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.brut_prim).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.hesaplama_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.komisyon_orani).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.komisyon_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.net_komisyon).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.odeme_tarihi).HasColumnType("datetime");
            entity.Property(e => e.stopaj_tutari).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.acente_kullanici).WithMany(p => p.KOMISYON_HESAPLARIs)
                .HasForeignKey(d => d.acente_kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KOMISYON___acent__3493CFA7");

            entity.HasOne(d => d.durum).WithMany(p => p.KOMISYON_HESAPLARIs)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KOMISYON___durum__3587F3E0");

            entity.HasOne(d => d.police).WithMany(p => p.KOMISYON_HESAPLARIs)
                .HasForeignKey(d => d.police_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KOMISYON___polic__339FAB6E");
        });

        modelBuilder.Entity<KULLANICILAR>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__KULLANIC__3213E83F9E2CBCE3");

            entity.ToTable("KULLANICILAR");

            entity.HasIndex(e => e.eposta, "UQ__KULLANIC__2549FEE6C5DAC599").IsUnique();

            entity.Property(e => e.ad).HasMaxLength(50);
            entity.Property(e => e.basarisiz_giris_sayisi).HasDefaultValue(0);
            entity.Property(e => e.eposta).HasMaxLength(100);
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.hesap_kilitlenme_tarihi).HasColumnType("datetime");
            entity.Property(e => e.kayit_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.sifre_hash).HasMaxLength(255);
            entity.Property(e => e.son_giris_tarihi).HasColumnType("datetime");
            entity.Property(e => e.soyad).HasMaxLength(50);
            entity.Property(e => e.telefon).HasMaxLength(20);

            entity.HasOne(d => d.durum).WithMany(p => p.KULLANICILARs)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KULLANICI__durum__412EB0B6");
        });

        modelBuilder.Entity<KULLANICI_ROLLER>(entity =>
        {
            entity.HasKey(e => new { e.kullanici_id, e.rol_id }).HasName("PK__KULLANIC__B3FDED46B4288BFB");

            entity.ToTable("KULLANICI_ROLLER");

            entity.Property(e => e.atanma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.bitis_tarihi).HasColumnType("datetime");

            entity.HasOne(d => d.kullanici).WithMany(p => p.KULLANICI_ROLLERs)
                .HasForeignKey(d => d.kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KULLANICI__kulla__5629CD9C");

            entity.HasOne(d => d.rol).WithMany(p => p.KULLANICI_ROLLERs)
                .HasForeignKey(d => d.rol_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KULLANICI__rol_i__571DF1D5");
        });

        modelBuilder.Entity<MUSTERILER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__MUSTERIL__3213E83FA69A30A2");

            entity.ToTable("MUSTERILER");

            entity.HasIndex(e => e.musteri_no, "UQ__MUSTERIL__966D2E8C6CAD6128").IsUnique();

            entity.Property(e => e.ad).HasMaxLength(50);
            entity.Property(e => e.adres_detay).HasMaxLength(255);
            entity.Property(e => e.adres_il).HasMaxLength(50);
            entity.Property(e => e.adres_ilce).HasMaxLength(50);
            entity.Property(e => e.adres_mahalle).HasMaxLength(50);
            entity.Property(e => e.aylik_gelir).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.blacklist_mi).HasDefaultValue(false);
            entity.Property(e => e.blacklist_nedeni).HasMaxLength(255);
            entity.Property(e => e.cep_telefonu).HasMaxLength(20);
            entity.Property(e => e.eposta).HasMaxLength(100);
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.kaydeden_kullanici).HasMaxLength(50);
            entity.Property(e => e.kayit_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.meslek).HasMaxLength(50);
            entity.Property(e => e.musteri_no).HasMaxLength(30);
            entity.Property(e => e.not_bilgileri).HasMaxLength(255);
            entity.Property(e => e.posta_kodu).HasMaxLength(10);
            entity.Property(e => e.sirket_adi).HasMaxLength(100);
            entity.Property(e => e.soyad).HasMaxLength(50);
            entity.Property(e => e.tc_kimlik_no).HasMaxLength(11);
            entity.Property(e => e.telefon).HasMaxLength(20);
            entity.Property(e => e.vergi_no).HasMaxLength(20);

            entity.HasOne(d => d.cinsiyet).WithMany(p => p.MUSTERILERcinsiyets)
                .HasForeignKey(d => d.cinsiyet_id)
                .HasConstraintName("FK__MUSTERILE__cinsi__6383C8BA");

            entity.HasOne(d => d.egitim_durumu).WithMany(p => p.MUSTERILERegitim_durumus)
                .HasForeignKey(d => d.egitim_durumu_id)
                .HasConstraintName("FK__MUSTERILE__egiti__656C112C");

            entity.HasOne(d => d.kullanici).WithMany(p => p.MUSTERILERs)
                .HasForeignKey(d => d.kullanici_id)
                .HasConstraintName("FK__MUSTERILE__kulla__619B8048");

            entity.HasOne(d => d.medeni_durum).WithMany(p => p.MUSTERILERmedeni_durums)
                .HasForeignKey(d => d.medeni_durum_id)
                .HasConstraintName("FK__MUSTERILE__meden__6477ECF3");

            entity.HasOne(d => d.tip).WithMany(p => p.MUSTERILERtips)
                .HasForeignKey(d => d.tip_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MUSTERILE__tip_i__628FA481");
        });

        modelBuilder.Entity<MUSTERI_ILETISIM_TERCIHLERI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__MUSTERI___3213E83F77352483");

            entity.ToTable("MUSTERI_ILETISIM_TERCIHLERI");

            entity.Property(e => e.email_bildirimi).HasDefaultValue(true);
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.sms_bildirimi).HasDefaultValue(true);

            entity.HasOne(d => d.musteri).WithMany(p => p.MUSTERI_ILETISIM_TERCIHLERIs)
                .HasForeignKey(d => d.musteri_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MUSTERI_I__muste__6E01572D");
        });

        modelBuilder.Entity<ODEMELER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__ODEMELER__3213E83F191C5CE6");

            entity.ToTable("ODEMELER");

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.banka_adi).HasMaxLength(50);
            entity.Property(e => e.banka_referans_no).HasMaxLength(50);
            entity.Property(e => e.fis_no).HasMaxLength(50);
            entity.Property(e => e.kart_son_4_hane).HasMaxLength(4);
            entity.Property(e => e.komisyon_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.makbuz_no).HasMaxLength(50);
            entity.Property(e => e.muhasebe_kayit_tarihi).HasColumnType("datetime");
            entity.Property(e => e.muhasebede_kaydedildi_mi).HasDefaultValue(false);
            entity.Property(e => e.odeme_tarihi).HasColumnType("datetime");
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.pos_referans_no).HasMaxLength(50);
            entity.Property(e => e.tahsil_tarihi).HasColumnType("datetime");
            entity.Property(e => e.tutar).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.durum).WithMany(p => p.ODEMELERdurums)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ODEMELER__durum___2FCF1A8A");

            entity.HasOne(d => d.musteri).WithMany(p => p.ODEMELERs)
                .HasForeignKey(d => d.musteri_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ODEMELER__muster__2BFE89A6");

            entity.HasOne(d => d.odeme_tipi).WithMany(p => p.ODEMELERodeme_tipis)
                .HasForeignKey(d => d.odeme_tipi_id)
                .HasConstraintName("FK__ODEMELER__odeme___2DE6D218");

            entity.HasOne(d => d.odeme_yontemi).WithMany(p => p.ODEMELERodeme_yontemis)
                .HasForeignKey(d => d.odeme_yontemi_id)
                .HasConstraintName("FK__ODEMELER__odeme___2EDAF651");

            entity.HasOne(d => d.police).WithMany(p => p.ODEMELERs)
                .HasForeignKey(d => d.police_id)
                .HasConstraintName("FK__ODEMELER__police__2B0A656D");

            entity.HasOne(d => d.tahsilat_yapan_kullanici).WithMany(p => p.ODEMELERs)
                .HasForeignKey(d => d.tahsilat_yapan_kullanici_id)
                .HasConstraintName("FK__ODEMELER__tahsil__2CF2ADDF");

            entity.HasOne(d => d.taksit).WithMany(p => p.ODEMELERs)
                .HasForeignKey(d => d.taksit_id)
                .HasConstraintName("FK__ODEMELER__taksit__2A164134");
        });

        modelBuilder.Entity<OTURUM_KAYITLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__OTURUM_K__3213E83F26126F0C");

            entity.ToTable("OTURUM_KAYITLARI");

            entity.Property(e => e.baslangic_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.bitis_tarihi).HasColumnType("datetime");
            entity.Property(e => e.cihaz_bilgisi).HasMaxLength(255);
            entity.Property(e => e.ip_adresi).HasMaxLength(50);
            entity.Property(e => e.oturum_token).HasMaxLength(255);
            entity.Property(e => e.tarayici_bilgisi).HasMaxLength(255);

            entity.HasOne(d => d.kullanici).WithMany(p => p.OTURUM_KAYITLARIs)
                .HasForeignKey(d => d.kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OTURUM_KA__kulla__5AEE82B9");
        });

        modelBuilder.Entity<POLICE_TEKLIFLERI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__POLICE_T__3213E83F46F858E7");

            entity.ToTable("POLICE_TEKLIFLERI");

            entity.HasIndex(e => e.teklif_no, "UQ__POLICE_T__A90A1AD96B121A3B").IsUnique();

            entity.Property(e => e.brut_prim).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.gecerlilik_tarihi).HasColumnType("datetime");
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.komisyon_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.net_prim).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.notlar).HasMaxLength(255);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.onay_tarihi).HasColumnType("datetime");
            entity.Property(e => e.onaylayan_kullanici).HasMaxLength(50);
            entity.Property(e => e.red_nedeni).HasMaxLength(255);
            entity.Property(e => e.risk_bilgileri).HasMaxLength(255);
            entity.Property(e => e.teklif_no).HasMaxLength(30);
            entity.Property(e => e.teklif_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.teminat_bilgileri).HasMaxLength(255);
            entity.Property(e => e.toplam_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.vergi_tutari).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.durum).WithMany(p => p.POLICE_TEKLIFLERIs)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TE__durum__03F0984C");

            entity.HasOne(d => d.musteri).WithMany(p => p.POLICE_TEKLIFLERIs)
                .HasForeignKey(d => d.musteri_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TE__muste__00200768");

            entity.HasOne(d => d.olusturan_kullanici).WithMany(p => p.POLICE_TEKLIFLERIs)
                .HasForeignKey(d => d.olusturan_kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TE__olust__02FC7413");

            entity.HasOne(d => d.police_turu).WithMany(p => p.POLICE_TEKLIFLERIs)
                .HasForeignKey(d => d.police_turu_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TE__polic__01142BA1");

            entity.HasOne(d => d.sigorta_sirketi).WithMany(p => p.POLICE_TEKLIFLERIs)
                .HasForeignKey(d => d.sigorta_sirketi_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TE__sigor__02084FDA");
        });

        modelBuilder.Entity<POLICE_TURLERI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__POLICE_T__3213E83F4A0077E7");

            entity.ToTable("POLICE_TURLERI");

            entity.HasIndex(e => e.urun_kodu, "UQ__POLICE_T__1359E5D30D80A072").IsUnique();

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.aktif_mi).HasDefaultValue(true);
            entity.Property(e => e.max_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.min_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.risk_faktorleri).HasMaxLength(255);
            entity.Property(e => e.urun_adi).HasMaxLength(100);
            entity.Property(e => e.urun_kodu).HasMaxLength(50);
            entity.Property(e => e.zorunlu_mi).HasDefaultValue(false);

            entity.HasOne(d => d.alt_kategori).WithMany(p => p.POLICE_TURLERIalt_kategoris)
                .HasForeignKey(d => d.alt_kategori_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TU__alt_k__797309D9");

            entity.HasOne(d => d.kategori).WithMany(p => p.POLICE_TURLERIkategoris)
                .HasForeignKey(d => d.kategori_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLICE_TU__kateg__787EE5A0");
        });

        modelBuilder.Entity<POLISELER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__POLISELE__3213E83F0ECB3EC8");

            entity.ToTable("POLISELER");

            entity.HasIndex(e => e.police_no, "UQ__POLISELE__68FFBEFB8254C983").IsUnique();

            entity.Property(e => e.baslangic_tarihi).HasColumnType("datetime");
            entity.Property(e => e.bitis_tarihi).HasColumnType("datetime");
            entity.Property(e => e.brut_prim).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.guncelleme_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.iptal_nedeni).HasMaxLength(255);
            entity.Property(e => e.iptal_tarihi).HasColumnType("datetime");
            entity.Property(e => e.komisyon_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.net_prim).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.notlar).HasMaxLength(255);
            entity.Property(e => e.ozel_sartlar).HasMaxLength(255);
            entity.Property(e => e.police_no).HasMaxLength(30);
            entity.Property(e => e.risk_bilgileri).HasMaxLength(255);
            entity.Property(e => e.tanzim_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.teminat_bilgileri).HasMaxLength(255);
            entity.Property(e => e.toplam_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.vergi_tutari).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.yenileme_hatirlatma_tarihi).HasColumnType("datetime");

            entity.HasOne(d => d.durum).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLISELER__durum__0E6E26BF");

            entity.HasOne(d => d.musteri).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.musteri_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLISELER__muste__0A9D95DB");

            entity.HasOne(d => d.police_turu).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.police_turu_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLISELER__polic__0B91BA14");

            entity.HasOne(d => d.sigorta_sirketi).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.sigorta_sirketi_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLISELER__sigor__0C85DE4D");

            entity.HasOne(d => d.tanzim_eden_kullanici).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.tanzim_eden_kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POLISELER__tanzi__0D7A0286");

            entity.HasOne(d => d.teklif).WithMany(p => p.POLISELERs)
                .HasForeignKey(d => d.teklif_id)
                .HasConstraintName("FK__POLISELER__tekli__09A971A2");
        });

        modelBuilder.Entity<RAPOR_SABLONLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__RAPOR_SA__3213E83FC1E43606");

            entity.ToTable("RAPOR_SABLONLARI");

            entity.Property(e => e.aktif_mi).HasDefaultValue(true);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.otomatik_mi).HasDefaultValue(false);
            entity.Property(e => e.parametreler).HasMaxLength(255);
            entity.Property(e => e.rapor_adi).HasMaxLength(100);

            entity.HasOne(d => d.cikti_formati).WithMany(p => p.RAPOR_SABLONLARIcikti_formatis)
                .HasForeignKey(d => d.cikti_formati_id)
                .HasConstraintName("FK__RAPOR_SAB__cikti__5224328E");

            entity.HasOne(d => d.olusturan_kullanici).WithMany(p => p.RAPOR_SABLONLARIs)
                .HasForeignKey(d => d.olusturan_kullanici_id)
                .HasConstraintName("FK__RAPOR_SAB__olust__540C7B00");

            entity.HasOne(d => d.rapor_tipi).WithMany(p => p.RAPOR_SABLONLARIrapor_tipis)
                .HasForeignKey(d => d.rapor_tipi_id)
                .HasConstraintName("FK__RAPOR_SAB__rapor__51300E55");

            entity.HasOne(d => d.zamanlama).WithMany(p => p.RAPOR_SABLONLARIzamanlamas)
                .HasForeignKey(d => d.zamanlama_id)
                .HasConstraintName("FK__RAPOR_SAB__zaman__531856C7");
        });

        modelBuilder.Entity<ROLLER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__ROLLER__3213E83FBF71C4FA");

            entity.ToTable("ROLLER");

            entity.HasIndex(e => e.rol_adi, "UQ__ROLLER__604EFF73B1DE8834").IsUnique();

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.aktif_mi).HasDefaultValue(true);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.rol_adi).HasMaxLength(50);
        });

        modelBuilder.Entity<SIFRE_SIFIRLAMA>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__SIFRE_SI__3213E83FB8C30544");

            entity.ToTable("SIFRE_SIFIRLAMA");

            entity.Property(e => e.ip_adresi).HasMaxLength(50);
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.son_kullanma_tarihi).HasColumnType("datetime");
            entity.Property(e => e.token).HasMaxLength(255);

            entity.HasOne(d => d.kullanici).WithMany(p => p.SIFRE_SIFIRLAMAs)
                .HasForeignKey(d => d.kullanici_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SIFRE_SIF__kulla__45F365D3");
        });

        modelBuilder.Entity<SIGORTA_SIRKETLERI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__SIGORTA___3213E83FD9ED5039");

            entity.ToTable("SIGORTA_SIRKETLERI");

            entity.HasIndex(e => e.sirket_kodu, "UQ__SIGORTA___C10CA9E75367FF1C").IsUnique();

            entity.Property(e => e.adres).HasMaxLength(255);
            entity.Property(e => e.aktif_mi).HasDefaultValue(true);
            entity.Property(e => e.eposta).HasMaxLength(100);
            entity.Property(e => e.komisyon_orani).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.sirket_adi).HasMaxLength(100);
            entity.Property(e => e.sirket_kodu).HasMaxLength(20);
            entity.Property(e => e.sozlesme_baslangic).HasColumnType("datetime");
            entity.Property(e => e.sozlesme_bitis).HasColumnType("datetime");
            entity.Property(e => e.telefon).HasMaxLength(20);
            entity.Property(e => e.vergi_no).HasMaxLength(20);
        });

        modelBuilder.Entity<SISTEM_LOGLARI>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__SISTEM_L__3213E83FED74E8C7");

            entity.ToTable("SISTEM_LOGLARI");

            entity.Property(e => e.aciklama).HasMaxLength(255);
            entity.Property(e => e.ip_adresi).HasMaxLength(50);
            entity.Property(e => e.islem_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.tablo_adi).HasMaxLength(50);
            entity.Property(e => e.tarayici_bilgisi).HasMaxLength(255);

            entity.HasOne(d => d.islem_tipi).WithMany(p => p.SISTEM_LOGLARIs)
                .HasForeignKey(d => d.islem_tipi_id)
                .HasConstraintName("FK__SISTEM_LO__islem__43D61337");

            entity.HasOne(d => d.kullanici).WithMany(p => p.SISTEM_LOGLARIs)
                .HasForeignKey(d => d.kullanici_id)
                .HasConstraintName("FK__SISTEM_LO__kulla__42E1EEFE");
        });

        modelBuilder.Entity<TAKSITLER>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__TAKSITLE__3213E83FE1900BE8");

            entity.ToTable("TAKSITLER");

            entity.Property(e => e.ana_para).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.gecikme_faizi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.odeme_tarihi).HasColumnType("datetime");
            entity.Property(e => e.olusturma_tarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.son_hatirlatma_tarihi).HasColumnType("datetime");
            entity.Property(e => e.toplam_tutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.vade_tarihi).HasColumnType("datetime");
            entity.Property(e => e.vergi_tutari).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.durum).WithMany(p => p.TAKSITLERdurums)
                .HasForeignKey(d => d.durum_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TAKSITLER__durum__245D67DE");

            entity.HasOne(d => d.hatirlatma_durumu).WithMany(p => p.TAKSITLERhatirlatma_durumus)
                .HasForeignKey(d => d.hatirlatma_durumu_id)
                .HasConstraintName("FK__TAKSITLER__hatir__25518C17");

            entity.HasOne(d => d.police).WithMany(p => p.TAKSITLERs)
                .HasForeignKey(d => d.police_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TAKSITLER__polic__236943A5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
