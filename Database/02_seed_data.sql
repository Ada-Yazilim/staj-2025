/****************************************************************************************
 * ADA YAZILIM - SİGORTA YÖNETİM PLATFORMU - VERİ TOHUMLAMA (SEEDING) SCRIPT'İ
 * Açıklama: Bu script, geliştirme ortamındaki veritabanını test edilebilir,
 * gerçekçi ve birbiriyle ilişkili verilerle doldurur.
 * KULLANIM: Tablolar oluşturulduktan sonra, boş veritabanı üzerinde bir kez çalıştırılmalıdır.
 ****************************************************************************************/

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. ADIM: ENUM VE TANIMLAMA VERİLERİ (DURUM_TANIMLARI)
    PRINT '1. Adım: DURUM_TANIMLARI tablosu dolduruluyor...';
    INSERT INTO DURUM_TANIMLARI (tablo_adi, alan_adi, deger_kodu, deger_aciklama, siralama, aktif_mi) VALUES
    ('KULLANICILAR', 'durum_id', 'AKTIF', 'Aktif Kullanıcı', 1, 1),
    ('KULLANICILAR', 'durum_id', 'PASIF', 'Pasif Kullanıcı', 2, 1),
    ('KULLANICILAR', 'durum_id', 'KILITLI', 'Hesap Kilitli', 3, 1),
    ('MUSTERILER', 'tip_id', 'BIREYSEL', 'Bireysel Müşteri', 1, 1),
    ('MUSTERILER', 'tip_id', 'KURUMSAL', 'Kurumsal Müşteri', 2, 1),
    ('MUSTERILER', 'cinsiyet_id', 'ERKEK', 'Erkek', 1, 1),
    ('MUSTERILER', 'cinsiyet_id', 'KADIN', 'Kadın', 2, 1),
    ('MUSTERILER', 'medeni_durum_id', 'BEKAR', 'Bekar', 1, 1),
    ('MUSTERILER', 'medeni_durum_id', 'EVLI', 'Evli', 2, 1),
    ('MUSTERILER', 'egitim_durumu_id', 'LISE_ALTI', 'Lise ve Altı', 1, 1),
    ('MUSTERILER', 'egitim_durumu_id', 'LISANS', 'Lisans', 2, 1),
    ('MUSTERILER', 'egitim_durumu_id', 'LISANSUSTU', 'Yüksek Lisans ve Üstü', 3, 1),
    ('POLICE_TURLERI', 'kategori_id', 'OTOMOTIV', 'Otomotiv Sigortaları', 1, 1),
    ('POLICE_TURLERI', 'kategori_id', 'SAGLIK', 'Sağlık Sigortaları', 2, 1),
    ('POLICE_TURLERI', 'kategori_id', 'KONUT', 'Konut Sigortaları', 3, 1),
    ('POLICE_TURLERI', 'alt_kategori_id', 'TRAFIK', 'Zorunlu Trafik Sigortası', 1, 1),
    ('POLICE_TURLERI', 'alt_kategori_id', 'KASKO', 'Kasko Sigortası', 2, 1),
    ('POLISELER', 'durum_id', 'AKTIF', 'Aktif', 1, 1),
    ('POLISELER', 'durum_id', 'IPTAL', 'İptal Edilmiş', 2, 1),
    ('POLISELER', 'durum_id', 'BITMIS', 'Süresi Bitmiş', 3, 1),
    ('HASAR_DOSYALAR', 'durum_id', 'BILDIRILDI', 'Bildirildi', 1, 1),
    ('HASAR_DOSYALAR', 'durum_id', 'INCELENIYOR', 'İnceleniyor', 2, 1),
    ('HASAR_DOSYALAR', 'durum_id', 'ONAYLANDI', 'Onaylandı', 3, 1),
    ('HASAR_DOSYALAR', 'durum_id', 'REDDEDILDI', 'Reddedildi', 4, 1),
    ('HASAR_DOSYALAR', 'durum_id', 'ODENDI', 'Ödendi', 5, 1),
    ('TAKSITLER', 'durum_id', 'ODENDI', 'Ödendi', 1, 1),
    ('TAKSITLER', 'durum_id', 'BEKLEMEDE', 'Ödeme Beklemede', 2, 1),
    ('TAKSITLER', 'durum_id', 'GECIKTI', 'Gecikmiş Ödeme', 3, 1),
    ('ODEMELER', 'durum_id', 'BASARILI', 'Başarılı', 1, 1),
    ('ODEMELER', 'durum_id', 'BASARISIZ', 'Başarısız', 2, 1),
    ('ODEMELER', 'odeme_yontemi_id', 'KREDI_KARTI', 'Kredi Kartı', 1, 1),
    ('ODEMELER', 'odeme_yontemi_id', 'BANKA_HAVALESI', 'Banka Havalesi', 2, 1);

    -- 2. ADIM: ÇEKİRDEK VARLIKLAR (ROLLER, SİGORTA ŞİRKETLERİ)
    PRINT '2. Adım: ROLLER ve SIGORTA_SIRKETLERI tabloları dolduruluyor...';
    INSERT INTO ROLLER (rol_adi, aciklama, aktif_mi) VALUES
    ('ADMIN', 'Sistem Yöneticisi - Tüm sisteme erişim hakkı vardır.', 1),
    ('ACENTE', 'Acente Kullanıcısı - Müşteri, poliçe ve hasar işlemleri yapabilir.', 1),
    ('KULLANICI', 'Normal Müşteri Kullanıcısı - Sadece kendi bilgilerini, poliçelerini ve hasarlarını yönetebilir.', 1);
    INSERT INTO SIGORTA_SIRKETLERI (sirket_adi, sirket_kodu, aktif_mi) VALUES
    ('Allianz Sigorta A.Ş.', 'ALLIANZ', 1),
    ('Axa Sigorta A.Ş.', 'AXA', 1),
    ('Anadolu Sigorta A.Ş.', 'ANADOLU', 1),
    ('Doğa Sigorta A.Ş.', 'DOGA', 1),
    ('Türkiye Sigorta A.Ş.', 'TURKIYE', 1);

    -- 3. ADIM: KULLANICILAR VE MÜŞTERİLER
    PRINT '3. Adım: KULLANICILAR ve MUSTERILER oluşturuluyor...';
    DECLARE @bcryptSifre NVARCHAR(MAX) = '$2a$11$UoZ.g2yWU13o3ANsAUvH.emqjLp7/fKkM2/umqjYx1s59vH3IF9d.'; -- 'Password123!' için örnek hash
    DECLARE @aktifKullaniciId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'KULLANICILAR' AND deger_kodu = 'AKTIF');
    DECLARE @bireyselMusteriId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'MUSTERILER' AND deger_kodu = 'BIREYSEL');
    DECLARE @kurumsalMusteriId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'MUSTERILER' AND deger_kodu = 'KURUMSAL');
    DECLARE @adminRoleId INT = (SELECT id FROM ROLLER WHERE rol_adi = 'ADMIN');
    DECLARE @acenteRoleId INT = (SELECT id FROM ROLLER WHERE rol_adi = 'ACENTE');
    DECLARE @kullaniciRoleId INT = (SELECT id FROM ROLLER WHERE rol_adi = 'KULLANICI');

    -- Admin Kullanıcısı
    INSERT INTO KULLANICILAR (ad, soyad, eposta, sifre_hash, telefon, durum_id, email_dogrulandi, telefon_dogrulandi)
    VALUES ('Admin', 'Sistem', 'admin@adayazilim.com', @bcryptSifre, '5551112233', @aktifKullaniciId, 1, 1);
    DECLARE @adminId INT = SCOPE_IDENTITY();
    INSERT INTO KULLANICI_ROLLER (kullanici_id, rol_id) VALUES (@adminId, @adminRoleId);

    -- Acente Kullanıcısı (Mert Kan)
    INSERT INTO KULLANICILAR (ad, soyad, eposta, sifre_hash, telefon, durum_id, email_dogrulandi, telefon_dogrulandi)
    VALUES ('Mert', 'Kan', 'mert.kan@adayazilim.com', @bcryptSifre, '5557778899', @aktifKullaniciId, 1, 1);
    DECLARE @mertId INT = SCOPE_IDENTITY();
    INSERT INTO KULLANICI_ROLLER (kullanici_id, rol_id) VALUES (@mertId, @acenteRoleId);

    -- Acente Kullanıcısı (Ayşe Çalışkan)
    INSERT INTO KULLANICILAR (ad, soyad, eposta, sifre_hash, telefon, durum_id, email_dogrulandi, telefon_dogrulandi)
    VALUES ('Ayşe', 'Çalışkan', 'ayse.caliskan@adayazilim.com', @bcryptSifre, '5554445566', @aktifKullaniciId, 1, 1);
    DECLARE @ayseId INT = SCOPE_IDENTITY();
    INSERT INTO KULLANICI_ROLLER (kullanici_id, rol_id) VALUES (@ayseId, @acenteRoleId);

    -- Müşteri Rolündeki Kullanıcı (Ahmet Yılmaz)
    INSERT INTO KULLANICILAR (ad, soyad, eposta, sifre_hash, telefon, durum_id, email_dogrulandi, telefon_dogrulandi)
    VALUES ('Ahmet', 'Yılmaz', 'ahmet.yilmaz@email.com', @bcryptSifre, '5321234567', @aktifKullaniciId, 1, 1);
    DECLARE @ahmetKullaniciId INT = SCOPE_IDENTITY();
    INSERT INTO KULLANICI_ROLLER (kullanici_id, rol_id) VALUES (@ahmetKullaniciId, @kullaniciRoleId);

    -- Müşteri Kayıtları
    INSERT INTO MUSTERILER (kullanici_id, musteri_no, tip_id, ad, soyad, tc_kimlik_no, eposta, cep_telefonu, kaydeden_kullanici)
    VALUES (@ahmetKullaniciId, 'M-2025-0001', @bireyselMusteriId, 'Ahmet', 'Yılmaz', '11111111111', 'ahmet.yilmaz@email.com', '5321234567', 'Kendisi');
    DECLARE @ahmetMusteriId INT = SCOPE_IDENTITY();
    INSERT INTO MUSTERILER (musteri_no, tip_id, ad, soyad, tc_kimlik_no, eposta, cep_telefonu, kaydeden_kullanici)
    VALUES ('M-2025-0002', @bireyselMusteriId, 'Zeynep', 'Kaya', '22222222222', 'zeynep.kaya@email.com', '5429876543', 'Mert Kan');
    DECLARE @zeynepMusteriId INT = SCOPE_IDENTITY();
    INSERT INTO MUSTERILER (musteri_no, tip_id, sirket_adi, vergi_no, eposta, telefon, kaydeden_kullanici)
    VALUES ('C-2025-0001', @kurumsalMusteriId, 'Ada Yazılım ve Danışmanlık Ltd. Şti.', '1112223334', 'info@adayazilim.com', '2124445566', 'Mert Kan');
    DECLARE @adaYazilimMusteriId INT = SCOPE_IDENTITY();

    -- 4. ADIM: POLİÇE, TAKSİT VE ÖDEME İŞLEMLERİ
    PRINT '4. Adım: Poliçeler, Taksitler ve Ödemeler oluşturuluyor...';
    INSERT INTO POLICE_TURLERI (kategori_id, alt_kategori_id, urun_adi, urun_kodu, aktif_mi) VALUES
    ((SELECT id FROM DURUM_TANIMLARI WHERE deger_kodu = 'OTOMOTIV'), (SELECT id FROM DURUM_TANIMLARI WHERE deger_kodu = 'KASKO'), 'Genişletilmiş Kasko', 'KASKO-GEN-01', 1),
    ((SELECT id FROM DURUM_TANIMLARI WHERE deger_kodu = 'OTOMOTIV'), (SELECT id FROM DURUM_TANIMLARI WHERE deger_kodu = 'TRAFIK'), 'Zorunlu Trafik Sigortası', 'TRAFIK-ZRN-01', 1);
    DECLARE @kaskoId INT = (SELECT id FROM POLICE_TURLERI WHERE urun_kodu = 'KASKO-GEN-01');
    DECLARE @aktifPoliceId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'POLISELER' AND deger_kodu = 'AKTIF');
    DECLARE @acenteMertId INT = (SELECT id FROM KULLANICILAR WHERE eposta = 'mert.kan@adayazilim.com');
    DECLARE @ahmetMusteriId INT = (SELECT id FROM MUSTERILER WHERE musteri_no = 'M-2025-0001');
    DECLARE @axaId INT = (SELECT id FROM SIGORTA_SIRKETLERI WHERE sirket_kodu = 'AXA');
    INSERT INTO POLISELER (police_no, musteri_id, police_turu_id, sigorta_sirketi_id, baslangic_tarihi, bitis_tarihi, toplam_tutar, durum_id, tanzim_eden_kullanici_id)
    VALUES ('AXA-2025-12345', @ahmetMusteriId, @kaskoId, @axaId, DATEADD(month, -2, GETDATE()), DATEADD(month, 10, GETDATE()), 9600.00, @aktifPoliceId, @acenteMertId);
    DECLARE @ahmetPoliceId INT = SCOPE_IDENTITY();
    DECLARE @odendiTaksitId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'TAKSITLER' AND deger_kodu = 'ODENDI');
    DECLARE @beklemedeTaksitId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'TAKSITLER' AND deger_kodu = 'BEKLEMEDE');
    DECLARE @basariliOdemeId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'ODEMELER' AND deger_kodu = 'BASARILI');
    DECLARE @kkOdemeId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'ODEMELER' AND deger_kodu = 'KREDI_KARTI');
    INSERT INTO TAKSITLER (police_id, taksit_no, vade_tarihi, toplam_tutar, durum_id, odeme_tarihi)
    VALUES (@ahmetPoliceId, 1, DATEADD(month, -2, GETDATE()), 3200.00, @odendiTaksitId, DATEADD(month, -2, GETDATE()));
    DECLARE @taksit1Id INT = SCOPE_IDENTITY();
    INSERT INTO ODEMELER (taksit_id, musteri_id, odeme_tarihi, tutar, odeme_yontemi_id, durum_id)
    VALUES (@taksit1Id, @ahmetMusteriId, DATEADD(month, -2, GETDATE()), 3200.00, @kkOdemeId, @basariliOdemeId);
    INSERT INTO TAKSITLER (police_id, taksit_no, vade_tarihi, toplam_tutar, durum_id, odeme_tarihi)
    VALUES (@ahmetPoliceId, 2, DATEADD(month, -1, GETDATE()), 3200.00, @odendiTaksitId, DATEADD(month, -1, GETDATE()));
    DECLARE @taksit2Id INT = SCOPE_IDENTITY();
    INSERT INTO ODEMELER (taksit_id, musteri_id, odeme_tarihi, tutar, odeme_yontemi_id, durum_id)
    VALUES (@taksit2Id, @ahmetMusteriId, DATEADD(month, -1, GETDATE()), 3200.00, @kkOdemeId, @basariliOdemeId);
    INSERT INTO TAKSITLER (police_id, taksit_no, vade_tarihi, toplam_tutar, durum_id)
    VALUES (@ahmetPoliceId, 3, GETDATE(), 3200.00, @beklemedeTaksitId);

    -- 5. ADIM: HASAR DOSYASI
    PRINT '5. Adım: Örnek Hasar Dosyası oluşturuluyor...';
    DECLARE @bildirildiHasarId INT = (SELECT id FROM DURUM_TANIMLARI WHERE tablo_adi = 'HASAR_DOSYALAR' AND deger_kodu = 'BILDIRILDI');
    DECLARE @ahmetKullaniciId INT = (SELECT id FROM KULLANICILAR WHERE eposta = 'ahmet.yilmaz@email.com');
    DECLARE @ahmetPoliceId INT = (SELECT P.id FROM POLISELER P JOIN MUSTERILER M ON P.musteri_id = M.id WHERE M.musteri_no = 'M-2025-0001');
    INSERT INTO HASAR_DOSYALAR (hasar_no, police_id, musteri_id, bildiren_kullanici_id, olay_tarihi, olay_aciklamasi, talep_edilen_tutar, durum_id)
    VALUES ('H-2025-0001', @ahmetPoliceId, (SELECT musteri_id FROM POLISELER WHERE id = @ahmetPoliceId), @ahmetKullaniciId, DATEADD(day, -10, GETDATE()), 'AVM otoparkında park halindeyken başka bir aracın çarpması sonucu sol ön çamurlukta göçük ve çizik oluştu.', 15000.00, @bildirildiHasarId);

    PRINT 'Tüm örnek veriler başarıyla eklendi!';
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT 'BİR HATA OLUŞTU, TÜM İŞLEMLER GERİ ALINDI!';
    PRINT 'Hata Numarası: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    PRINT 'Hata Mesajı: ' + ERROR_MESSAGE();
    PRINT 'Hatanın Olduğu Satır: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
END CATCH 