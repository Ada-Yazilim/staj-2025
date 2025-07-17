/****************************************************************************************
 * ADA YAZILIM - SİGORTA YÖNETİM PLATFORMU - VERİTABANI OLUŞTURMA SCRIPT'İ
 * Açıklama: Bu script, temel tablo ve ilişkileri oluşturur. MSSQL uyumludur.
 ****************************************************************************************/

-- ENUM VE TANIMLAR
CREATE TABLE DURUM_TANIMLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tablo_adi NVARCHAR(50) NOT NULL,
    alan_adi NVARCHAR(50) NOT NULL,
    deger_kodu NVARCHAR(50) NOT NULL,
    deger_aciklama NVARCHAR(255) NOT NULL,
    siralama INT NOT NULL,
    aktif_mi BIT NOT NULL DEFAULT 1,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE()
);

-- KULLANICI YÖNETİMİ
CREATE TABLE KULLANICILAR (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ad NVARCHAR(50) NOT NULL,
    soyad NVARCHAR(50) NOT NULL,
    eposta NVARCHAR(100) NOT NULL UNIQUE,
    sifre_hash NVARCHAR(255) NOT NULL,
    telefon NVARCHAR(20),
    durum_id INT NOT NULL,
    email_dogrulandi BIT NOT NULL DEFAULT 0,
    telefon_dogrulandi BIT NOT NULL DEFAULT 0,
    son_giris_tarihi DATETIME,
    basarisiz_giris_sayisi INT DEFAULT 0,
    hesap_kilitlenme_tarihi DATETIME,
    kayit_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE SIFRE_SIFIRLAMA (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_id INT NOT NULL,
    token NVARCHAR(255) NOT NULL,
    son_kullanma_tarihi DATETIME NOT NULL,
    kullanildi_mi BIT NOT NULL DEFAULT 0,
    ip_adresi NVARCHAR(50),
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id)
);

CREATE TABLE DOGRULAMA_KODLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_id INT NOT NULL,
    kod NVARCHAR(20) NOT NULL,
    tip_id INT NOT NULL,
    amac_id INT NOT NULL,
    son_kullanma_tarihi DATETIME NOT NULL,
    kullanildi_mi BIT NOT NULL DEFAULT 0,
    deneme_sayisi INT DEFAULT 0,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (tip_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (amac_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE ROLLER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    rol_adi NVARCHAR(50) NOT NULL UNIQUE,
    aciklama NVARCHAR(255),
    aktif_mi BIT NOT NULL DEFAULT 1,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE KULLANICI_ROLLER (
    kullanici_id INT NOT NULL,
    rol_id INT NOT NULL,
    atanma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    bitis_tarihi DATETIME,
    PRIMARY KEY (kullanici_id, rol_id),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (rol_id) REFERENCES ROLLER(id)
);

CREATE TABLE OTURUM_KAYITLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_id INT NOT NULL,
    oturum_token NVARCHAR(255) NOT NULL,
    ip_adresi NVARCHAR(50),
    tarayici_bilgisi NVARCHAR(255),
    cihaz_bilgisi NVARCHAR(255),
    baslangic_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    bitis_tarihi DATETIME,
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id)
);

-- MÜŞTERİ YÖNETİMİ
CREATE TABLE MUSTERILER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_id INT,
    musteri_no NVARCHAR(30) NOT NULL UNIQUE,
    tip_id INT NOT NULL,
    ad NVARCHAR(50),
    soyad NVARCHAR(50),
    sirket_adi NVARCHAR(100),
    vergi_no NVARCHAR(20),
    tc_kimlik_no NVARCHAR(11),
    eposta NVARCHAR(100),
    telefon NVARCHAR(20),
    cep_telefonu NVARCHAR(20),
    dogum_tarihi DATE,
    cinsiyet_id INT,
    medeni_durum_id INT,
    meslek NVARCHAR(50),
    egitim_durumu_id INT,
    aylik_gelir DECIMAL(18,2),
    adres_il NVARCHAR(50),
    adres_ilce NVARCHAR(50),
    adres_mahalle NVARCHAR(50),
    adres_detay NVARCHAR(255),
    posta_kodu NVARCHAR(10),
    not_bilgileri NVARCHAR(255),
    blacklist_mi BIT DEFAULT 0,
    blacklist_nedeni NVARCHAR(255),
    kayit_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    kaydeden_kullanici NVARCHAR(50),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (tip_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (cinsiyet_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (medeni_durum_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (egitim_durumu_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE MUSTERI_ILETISIM_TERCIHLERI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    musteri_id INT NOT NULL,
    email_bildirimi BIT NOT NULL DEFAULT 1,
    sms_bildirimi BIT NOT NULL DEFAULT 1,
    whatsapp_bildirimi BIT NOT NULL DEFAULT 0,
    arama_bildirimi BIT NOT NULL DEFAULT 0,
    pazarlama_onayi BIT NOT NULL DEFAULT 0,
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (musteri_id) REFERENCES MUSTERILER(id)
);

-- POLİÇE VE ÜRÜN YÖNETİMİ
CREATE TABLE SIGORTA_SIRKETLERI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    sirket_adi NVARCHAR(100) NOT NULL,
    sirket_kodu NVARCHAR(20) NOT NULL UNIQUE,
    vergi_no NVARCHAR(20),
    telefon NVARCHAR(20),
    eposta NVARCHAR(100),
    adres NVARCHAR(255),
    aktif_mi BIT NOT NULL DEFAULT 1,
    komisyon_orani DECIMAL(5,2),
    sozlesme_baslangic DATETIME,
    sozlesme_bitis DATETIME
);

CREATE TABLE POLICE_TURLERI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kategori_id INT NOT NULL,
    alt_kategori_id INT NOT NULL,
    urun_adi NVARCHAR(100) NOT NULL,
    urun_kodu NVARCHAR(50) NOT NULL UNIQUE,
    aciklama NVARCHAR(255),
    zorunlu_mi BIT DEFAULT 0,
    min_tutar DECIMAL(18,2),
    max_tutar DECIMAL(18,2),
    min_sure_gun INT,
    max_sure_gun INT,
    risk_faktorleri NVARCHAR(255),
    aktif_mi BIT NOT NULL DEFAULT 1,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (kategori_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (alt_kategori_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE POLICE_TEKLIFLERI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    teklif_no NVARCHAR(30) NOT NULL UNIQUE,
    musteri_id INT NOT NULL,
    police_turu_id INT NOT NULL,
    sigorta_sirketi_id INT NOT NULL,
    olusturan_kullanici_id INT NOT NULL,
    risk_bilgileri NVARCHAR(255),
    teminat_bilgileri NVARCHAR(255),
    brut_prim DECIMAL(18,2),
    net_prim DECIMAL(18,2),
    komisyon_tutari DECIMAL(18,2),
    vergi_tutari DECIMAL(18,2),
    toplam_tutar DECIMAL(18,2),
    taksit_sayisi INT,
    teklif_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    gecerlilik_tarihi DATETIME,
    durum_id INT NOT NULL,
    red_nedeni NVARCHAR(255),
    onay_tarihi DATETIME,
    onaylayan_kullanici NVARCHAR(50),
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    notlar NVARCHAR(255),
    FOREIGN KEY (musteri_id) REFERENCES MUSTERILER(id),
    FOREIGN KEY (police_turu_id) REFERENCES POLICE_TURLERI(id),
    FOREIGN KEY (sigorta_sirketi_id) REFERENCES SIGORTA_SIRKETLERI(id),
    FOREIGN KEY (olusturan_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE POLISELER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    police_no NVARCHAR(30) NOT NULL UNIQUE,
    teklif_id INT,
    musteri_id INT NOT NULL,
    police_turu_id INT NOT NULL,
    sigorta_sirketi_id INT NOT NULL,
    tanzim_eden_kullanici_id INT NOT NULL,
    risk_bilgileri NVARCHAR(255),
    teminat_bilgileri NVARCHAR(255),
    baslangic_tarihi DATETIME NOT NULL,
    bitis_tarihi DATETIME NOT NULL,
    brut_prim DECIMAL(18,2),
    net_prim DECIMAL(18,2),
    komisyon_tutari DECIMAL(18,2),
    vergi_tutari DECIMAL(18,2),
    toplam_tutar DECIMAL(18,2),
    taksit_sayisi INT,
    durum_id INT NOT NULL,
    iptal_nedeni NVARCHAR(255),
    iptal_tarihi DATETIME,
    yenileme_hatirlatma_tarihi DATETIME,
    ozel_sartlar NVARCHAR(255),
    notlar NVARCHAR(255),
    tanzim_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (teklif_id) REFERENCES POLICE_TEKLIFLERI(id),
    FOREIGN KEY (musteri_id) REFERENCES MUSTERILER(id),
    FOREIGN KEY (police_turu_id) REFERENCES POLICE_TURLERI(id),
    FOREIGN KEY (sigorta_sirketi_id) REFERENCES SIGORTA_SIRKETLERI(id),
    FOREIGN KEY (tanzim_eden_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

-- HASAR YÖNETİMİ
CREATE TABLE HASAR_DOSYALAR (
    id INT IDENTITY(1,1) PRIMARY KEY,
    hasar_no NVARCHAR(30) NOT NULL UNIQUE,
    police_id INT NOT NULL,
    musteri_id INT NOT NULL,
    bildiren_kullanici_id INT NOT NULL,
    olay_tarihi DATETIME NOT NULL,
    olay_saati NVARCHAR(10),
    olay_yeri_il NVARCHAR(50),
    olay_yeri_ilce NVARCHAR(50),
    olay_yeri_detay NVARCHAR(255),
    olay_tipi_id INT,
    olay_aciklamasi NVARCHAR(255),
    polis_rapor_no NVARCHAR(50),
    polis_raporu_var_mi BIT DEFAULT 0,
    tanik_bilgileri NVARCHAR(255),
    durum_id INT NOT NULL,
    talep_edilen_tutar DECIMAL(18,2),
    ekspertiz_tutari DECIMAL(18,2),
    onaylanan_tutar DECIMAL(18,2),
    odenen_tutar DECIMAL(18,2),
    red_nedeni NVARCHAR(255),
    sorumlu_eksper_id INT,
    ekspertiz_tarihi DATETIME,
    karar_tarihi DATETIME,
    odeme_tarihi DATETIME,
    onaylayan_kullanici NVARCHAR(50),
    bildirim_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    guncelleme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    notlar NVARCHAR(255),
    FOREIGN KEY (police_id) REFERENCES POLISELER(id),
    FOREIGN KEY (musteri_id) REFERENCES MUSTERILER(id),
    FOREIGN KEY (bildiren_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (olay_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (sorumlu_eksper_id) REFERENCES KULLANICILAR(id)
);

CREATE TABLE HASAR_TAKIP_NOTLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    hasar_id INT NOT NULL,
    kullanici_id INT NOT NULL,
    not_metni NVARCHAR(255) NOT NULL,
    not_tipi_id INT,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (hasar_id) REFERENCES HASAR_DOSYALAR(id),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (not_tipi_id) REFERENCES DURUM_TANIMLARI(id)
);

-- ÖDEME VE FİNANS
CREATE TABLE TAKSITLER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    police_id INT NOT NULL,
    taksit_no INT NOT NULL,
    vade_tarihi DATETIME NOT NULL,
    ana_para DECIMAL(18,2),
    vergi_tutari DECIMAL(18,2),
    toplam_tutar DECIMAL(18,2) NOT NULL,
    durum_id INT NOT NULL,
    odeme_tarihi DATETIME,
    gecikme_faizi DECIMAL(18,2),
    gecikme_gun_sayisi INT,
    hatirlatma_durumu_id INT,
    son_hatirlatma_tarihi DATETIME,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (police_id) REFERENCES POLISELER(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (hatirlatma_durumu_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE ODEMELER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    taksit_id INT,
    police_id INT,
    musteri_id INT NOT NULL,
    tahsilat_yapan_kullanici_id INT,
    odeme_tipi_id INT,
    odeme_tarihi DATETIME NOT NULL,
    tutar DECIMAL(18,2) NOT NULL,
    odeme_yontemi_id INT,
    durum_id INT NOT NULL,
    banka_adi NVARCHAR(50),
    kart_son_4_hane NVARCHAR(4),
    pos_referans_no NVARCHAR(50),
    banka_referans_no NVARCHAR(50),
    makbuz_no NVARCHAR(50),
    komisyon_tutari DECIMAL(18,2),
    aciklama NVARCHAR(255),
    tahsil_tarihi DATETIME,
    muhasebe_kayit_tarihi DATETIME,
    muhasebede_kaydedildi_mi BIT DEFAULT 0,
    fis_no NVARCHAR(50),
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (taksit_id) REFERENCES TAKSITLER(id),
    FOREIGN KEY (police_id) REFERENCES POLISELER(id),
    FOREIGN KEY (musteri_id) REFERENCES MUSTERILER(id),
    FOREIGN KEY (tahsilat_yapan_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (odeme_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (odeme_yontemi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE KOMISYON_HESAPLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    police_id INT NOT NULL,
    acente_kullanici_id INT NOT NULL,
    brut_prim DECIMAL(18,2),
    komisyon_orani DECIMAL(5,2),
    komisyon_tutari DECIMAL(18,2),
    stopaj_tutari DECIMAL(18,2),
    net_komisyon DECIMAL(18,2),
    durum_id INT NOT NULL,
    hesaplama_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    odeme_tarihi DATETIME,
    aciklama NVARCHAR(255),
    FOREIGN KEY (police_id) REFERENCES POLISELER(id),
    FOREIGN KEY (acente_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

-- DOKÜMAN YÖNETİMİ
CREATE TABLE DOKUMANLAR (
    id INT IDENTITY(1,1) PRIMARY KEY,
    iliski_id INT,
    iliski_tipi_id INT,
    dosya_adi NVARCHAR(255) NOT NULL,
    orijinal_dosya_adi NVARCHAR(255),
    dosya_yolu NVARCHAR(255),
    dosya_tipi_id INT,
    dosya_boyutu INT,
    mime_type NVARCHAR(100),
    hash_degeri NVARCHAR(255),
    kategori_id INT,
    aciklama NVARCHAR(255),
    zorunlu_mu BIT DEFAULT 0,
    yukleyen_kullanici_id INT,
    yuklenme_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    son_erisim_tarihi DATETIME,
    silinebilir_mi BIT DEFAULT 1,
    gizlilik_seviyesi_id INT,
    FOREIGN KEY (iliski_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (dosya_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (kategori_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (gizlilik_seviyesi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (yukleyen_kullanici_id) REFERENCES KULLANICILAR(id)
);

-- SİSTEM VE LOG KAYITLARI
CREATE TABLE SISTEM_LOGLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_id INT,
    islem_tipi_id INT,
    tablo_adi NVARCHAR(50),
    kayit_id INT,
    eski_deger NVARCHAR(MAX),
    yeni_deger NVARCHAR(MAX),
    ip_adresi NVARCHAR(50),
    tarayici_bilgisi NVARCHAR(255),
    islem_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    aciklama NVARCHAR(255),
    FOREIGN KEY (kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (islem_tipi_id) REFERENCES DURUM_TANIMLARI(id)
);

CREATE TABLE BILDIRIMLER (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alici_kullanici_id INT NOT NULL,
    bildirim_tipi_id INT,
    baslik NVARCHAR(100) NOT NULL,
    icerik NVARCHAR(255) NOT NULL,
    oncelik_id INT,
    okundu_mu BIT DEFAULT 0,
    gonderim_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    okunma_tarihi DATETIME,
    durum_id INT NOT NULL,
    hata_mesaji NVARCHAR(255),
    FOREIGN KEY (alici_kullanici_id) REFERENCES KULLANICILAR(id),
    FOREIGN KEY (bildirim_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (oncelik_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (durum_id) REFERENCES DURUM_TANIMLARI(id)
);

-- RAPORLAMA VE ANALİTİK
CREATE TABLE RAPOR_SABLONLARI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    rapor_adi NVARCHAR(100) NOT NULL,
    rapor_tipi_id INT,
    sql_sorgusu NVARCHAR(MAX),
    parametreler NVARCHAR(255),
    cikti_formati_id INT,
    otomatik_mi BIT DEFAULT 0,
    zamanlama_id INT,
    aktif_mi BIT NOT NULL DEFAULT 1,
    olusturan_kullanici_id INT,
    olusturma_tarihi DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (rapor_tipi_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (cikti_formati_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (zamanlama_id) REFERENCES DURUM_TANIMLARI(id),
    FOREIGN KEY (olusturan_kullanici_id) REFERENCES KULLANICILAR(id)
); 