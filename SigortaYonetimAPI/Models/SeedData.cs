using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SigortaYonetimAPI.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SigortaYonetimDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                // Durum tanımlarını oluştur
                await SeedDurumTanimlari(context, logger);

                // Sigorta şirketlerini oluştur
                await SeedSigortaSirketleri(context, logger);

                // Poliçe türlerini oluştur
                await SeedPoliceTurleri(context, logger);

                // Varsayılan admin kullanıcısını oluştur
                await SeedDefaultAdmin(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Seed verileri oluşturulurken hata oluştu.");
                throw;
            }
        }

        private static async Task SeedDurumTanimlari(SigortaYonetimDbContext context, ILogger<Program> logger)
        {

            var durumlar = new List<DURUM_TANIMLARI>
            {
                // KULLANICILAR tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "KULLANICILAR", alan_adi = "durum", deger_kodu = "AKTIF", deger_aciklama = "Aktif", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "KULLANICILAR", alan_adi = "durum", deger_kodu = "PASIF", deger_aciklama = "Pasif", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "KULLANICILAR", alan_adi = "durum", deger_kodu = "KILITLI", deger_aciklama = "Kilitli", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // MUSTERILER tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "tip_id", deger_kodu = "BIREYSEL", deger_aciklama = "Bireysel Müşteri", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "tip_id", deger_kodu = "KURUMSAL", deger_aciklama = "Kurumsal Müşteri", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "cinsiyet_id", deger_kodu = "ERKEK", deger_aciklama = "Erkek", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "cinsiyet_id", deger_kodu = "KADIN", deger_aciklama = "Kadın", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "medeni_durum_id", deger_kodu = "BEKAR", deger_aciklama = "Bekar", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "medeni_durum_id", deger_kodu = "EVLI", deger_aciklama = "Evli", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "medeni_durum_id", deger_kodu = "BOSANMIŞ", deger_aciklama = "Boşanmış", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "ILKOKUL", deger_aciklama = "İlkokul", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "ORTAOKUL", deger_aciklama = "Ortaokul", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "LISE", deger_aciklama = "Lise", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "UNIVERSITE", deger_aciklama = "Üniversite", siralama = 4, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "YUKSEKLISANS", deger_aciklama = "Yüksek Lisans", siralama = 5, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "MUSTERILER", alan_adi = "egitim_durumu_id", deger_kodu = "DOKTORA", deger_aciklama = "Doktora", siralama = 6, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // POLICE_TEKLIFLERI tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "POLICE_TEKLIFLERI", alan_adi = "durum", deger_kodu = "BEKLEMEDE", deger_aciklama = "Beklemede", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "POLICE_TEKLIFLERI", alan_adi = "durum", deger_kodu = "ONAYLANDI", deger_aciklama = "Onaylandı", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "POLICE_TEKLIFLERI", alan_adi = "durum", deger_kodu = "REDDEDILDI", deger_aciklama = "Reddedildi", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "POLICE_TEKLIFLERI", alan_adi = "durum", deger_kodu = "SURESI_DOLDU", deger_aciklama = "Süresi Doldu", siralama = 4, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // POLISELER tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "POLISELER", alan_adi = "durum", deger_kodu = "AKTIF", deger_aciklama = "Aktif", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "POLISELER", alan_adi = "durum", deger_kodu = "IPTAL", deger_aciklama = "İptal", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "POLISELER", alan_adi = "durum", deger_kodu = "SURESI_DOLDU", deger_aciklama = "Süresi Doldu", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // ODEMELER tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "ODEMELER", alan_adi = "durum", deger_kodu = "BEKLEMEDE", deger_aciklama = "Beklemede", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "ODEMELER", alan_adi = "durum", deger_kodu = "ODENDI", deger_aciklama = "Ödendi", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "ODEMELER", alan_adi = "durum", deger_kodu = "IPTAL", deger_aciklama = "İptal", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "ODEMELER", alan_adi = "durum", deger_kodu = "GECIKTI", deger_aciklama = "Gecikti", siralama = 4, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // TAKSITLER tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "TAKSITLER", alan_adi = "durum", deger_kodu = "BEKLEMEDE", deger_aciklama = "Beklemede", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "TAKSITLER", alan_adi = "durum", deger_kodu = "ODENDI", deger_aciklama = "Ödendi", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "TAKSITLER", alan_adi = "durum", deger_kodu = "GECIKTI", deger_aciklama = "Gecikti", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // HASAR_DOSYALAR tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "HASAR_DOSYALAR", alan_adi = "durum", deger_kodu = "BILDIRILDI", deger_aciklama = "Bildirildi", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "HASAR_DOSYALAR", alan_adi = "durum", deger_kodu = "INCELEMEDE", deger_aciklama = "İnceleniyor", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "HASAR_DOSYALAR", alan_adi = "durum", deger_kodu = "ONAYLANDI", deger_aciklama = "Onaylandı", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "HASAR_DOSYALAR", alan_adi = "durum", deger_kodu = "REDDEDILDI", deger_aciklama = "Reddedildi", siralama = 4, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "HASAR_DOSYALAR", alan_adi = "durum", deger_kodu = "ODENDI", deger_aciklama = "Ödendi", siralama = 5, aktif_mi = true, olusturma_tarihi = DateTime.Now },

                // KOMISYON_HESAPLARI tablosu durumları
                new DURUM_TANIMLARI { tablo_adi = "KOMISYON_HESAPLARI", alan_adi = "durum", deger_kodu = "HESAPLANDI", deger_aciklama = "Hesaplandı", siralama = 1, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "KOMISYON_HESAPLARI", alan_adi = "durum", deger_kodu = "ODENDI", deger_aciklama = "Ödendi", siralama = 2, aktif_mi = true, olusturma_tarihi = DateTime.Now },
                new DURUM_TANIMLARI { tablo_adi = "KOMISYON_HESAPLARI", alan_adi = "durum", deger_kodu = "IPTAL", deger_aciklama = "İptal", siralama = 3, aktif_mi = true, olusturma_tarihi = DateTime.Now }
            };

            foreach (var durum in durumlar)
            {
                var existingDurum = await context.DURUM_TANIMLARIs
                    .FirstOrDefaultAsync(d => d.tablo_adi == durum.tablo_adi && 
                                             d.alan_adi == durum.alan_adi && 
                                             d.deger_kodu == durum.deger_kodu);

                if (existingDurum == null)
                {
                    context.DURUM_TANIMLARIs.Add(durum);
                }
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Durum tanımları oluşturuldu.");
        }

        private static async Task SeedSigortaSirketleri(SigortaYonetimDbContext context, ILogger<Program> logger)
        {
            logger.LogInformation("Sigorta şirketleri oluşturuluyor...");

            var sigortaSirketleri = new List<SIGORTA_SIRKETLERI>
            {
                new SIGORTA_SIRKETLERI
                {
                    sirket_kodu = "AKSIGORTA",
                    sirket_adi = "Aksigorta A.Ş.",
                    adres = "Maslak Mahallesi, Büyükdere Caddesi No:201, 34398 Sarıyer/İstanbul",
                    telefon = "0212 315 30 00",
                    eposta = "info@aksigorta.com.tr",
                    vergi_no = "1234567890",
                    komisyon_orani = 15.00m,
                    sozlesme_baslangic = DateTime.Now.AddYears(-2),
                    sozlesme_bitis = DateTime.Now.AddYears(3),
                    aktif_mi = true
                },
                new SIGORTA_SIRKETLERI
                {
                    sirket_kodu = "ANADOLU",
                    sirket_adi = "Anadolu Anonim Türk Sigorta Şirketi",
                    adres = "Barbaros Mahallesi, Ardıç Sokak No:2, 34746 Ataşehir/İstanbul",
                    telefon = "0216 571 50 00",
                    eposta = "info@anadolusigorta.com.tr",
                    vergi_no = "2345678901",
                    komisyon_orani = 12.50m,
                    sozlesme_baslangic = DateTime.Now.AddYears(-1),
                    sozlesme_bitis = DateTime.Now.AddYears(4),
                    aktif_mi = true
                },
                new SIGORTA_SIRKETLERI
                {
                    sirket_kodu = "ALLIANZ",
                    sirket_adi = "Allianz Sigorta A.Ş.",
                    adres = "Levent Mahallesi, Büyükdere Caddesi No:199, 34394 Beşiktaş/İstanbul",
                    telefon = "0212 385 50 00",
                    eposta = "info@allianz.com.tr",
                    vergi_no = "3456789012",
                    komisyon_orani = 18.00m,
                    sozlesme_baslangic = DateTime.Now.AddYears(-3),
                    sozlesme_bitis = DateTime.Now.AddYears(2),
                    aktif_mi = true
                },
                new SIGORTA_SIRKETLERI
                {
                    sirket_kodu = "AXA",
                    sirket_adi = "AXA Sigorta A.Ş.",
                    adres = "Maslak Mahallesi, Büyükdere Caddesi No:185, 34398 Sarıyer/İstanbul",
                    telefon = "0212 315 15 00",
                    eposta = "info@axa.com.tr",
                    vergi_no = "4567890123",
                    komisyon_orani = 16.50m,
                    sozlesme_baslangic = DateTime.Now.AddYears(-2),
                    sozlesme_bitis = DateTime.Now.AddYears(3),
                    aktif_mi = true
                },
                new SIGORTA_SIRKETLERI
                {
                    sirket_kodu = "MAPFRE",
                    sirket_adi = "Mapfre Genel Sigorta A.Ş.",
                    adres = "Maslak Mahallesi, Büyükdere Caddesi No:201, 34398 Sarıyer/İstanbul",
                    telefon = "0212 315 30 00",
                    eposta = "info@mapfre.com.tr",
                    vergi_no = "5678901234",
                    komisyon_orani = 14.00m,
                    sozlesme_baslangic = DateTime.Now.AddYears(-1),
                    sozlesme_bitis = DateTime.Now.AddYears(4),
                    aktif_mi = true
                }
            };

            foreach (var sirket in sigortaSirketleri)
            {
                var existingSirket = await context.SIGORTA_SIRKETLERIs
                    .FirstOrDefaultAsync(s => s.sirket_kodu == sirket.sirket_kodu);

                if (existingSirket == null)
                {
                    context.SIGORTA_SIRKETLERIs.Add(sirket);
                }
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Sigorta şirketleri oluşturuldu.");
        }

        private static async Task SeedPoliceTurleri(SigortaYonetimDbContext context, ILogger<Program> logger)
        {
            logger.LogInformation("Poliçe türleri oluşturuluyor...");

            var policeTurleri = new List<POLICE_TURLERI>
            {
                // Kasko Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "KASKO",
                    urun_adi = "Kasko Sigortası",
                    aciklama = "Araç sahiplerinin araçlarını çeşitli risklere karşı koruyan sigorta türü",
                    kategori_id = 1, // Motorlu Araçlar
                    alt_kategori_id = 1, // Kasko
                    min_tutar = 1000.00m,
                    max_tutar = 1000000.00m,
                    risk_faktorleri = "Araç yaşı, sürücü deneyimi, kullanım amacı, bölge",
                    zorunlu_mi = false,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                },
                // Trafik Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "TRAFIK",
                    urun_adi = "Trafik Sigortası",
                    aciklama = "Motorlu araçlar için zorunlu olan üçüncü şahıs sorumluluk sigortası",
                    kategori_id = 1, // Motorlu Araçlar
                    alt_kategori_id = 2, // Trafik
                    min_tutar = 500.00m,
                    max_tutar = 50000.00m,
                    risk_faktorleri = "Araç tipi, motor hacmi, kullanım amacı",
                    zorunlu_mi = true,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                },
                // Konut Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "KONUT",
                    urun_adi = "Konut Sigortası",
                    aciklama = "Ev sahiplerinin konutlarını çeşitli risklere karşı koruyan sigorta",
                    kategori_id = 2, // Konut
                    alt_kategori_id = 3, // Konut
                    min_tutar = 200.00m,
                    max_tutar = 500000.00m,
                    risk_faktorleri = "Konut tipi, yaşı, bölge, güvenlik önlemleri",
                    zorunlu_mi = false,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                },
                // Sağlık Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "SAGLIK",
                    urun_adi = "Sağlık Sigortası",
                    aciklama = "Bireylerin sağlık giderlerini karşılayan sigorta türü",
                    kategori_id = 3, // Sağlık
                    alt_kategori_id = 4, // Sağlık
                    min_tutar = 300.00m,
                    max_tutar = 100000.00m,
                    risk_faktorleri = "Yaş, cinsiyet, sağlık durumu, meslek",
                    zorunlu_mi = false,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                },
                // Hayat Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "HAYAT",
                    urun_adi = "Hayat Sigortası",
                    aciklama = "Sigortalının vefatı durumunda yakınlarına maddi destek sağlayan sigorta",
                    kategori_id = 4, // Hayat
                    alt_kategori_id = 5, // Hayat
                    min_tutar = 500.00m,
                    max_tutar = 2000000.00m,
                    risk_faktorleri = "Yaş, cinsiyet, sağlık durumu, meslek, sigara kullanımı",
                    zorunlu_mi = false,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                },
                // İşyeri Sigortası
                new POLICE_TURLERI
                {
                    urun_kodu = "ISYERI",
                    urun_adi = "İşyeri Sigortası",
                    aciklama = "İşyerlerini çeşitli risklere karşı koruyan sigorta türü",
                    kategori_id = 5, // Ticari
                    alt_kategori_id = 6, // İşyeri
                    min_tutar = 1000.00m,
                    max_tutar = 1000000.00m,
                    risk_faktorleri = "İşyeri tipi, büyüklüğü, bölge, güvenlik önlemleri",
                    zorunlu_mi = false,
                    aktif_mi = true,
                    olusturma_tarihi = DateTime.Now
                }
            };

            foreach (var policeTuru in policeTurleri)
            {
                var existingPoliceTuru = await context.POLICE_TURLERIs
                    .FirstOrDefaultAsync(p => p.urun_kodu == policeTuru.urun_kodu);

                if (existingPoliceTuru == null)
                {
                    context.POLICE_TURLERIs.Add(policeTuru);
                }
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Poliçe türleri oluşturuldu.");
        }

        private static async Task SeedDefaultAdmin(SigortaYonetimDbContext context, ILogger<Program> logger)
        {
            logger.LogInformation("Varsayılan admin kullanıcısı oluşturuluyor...");

            // Önce KULLANICILAR tablosuna admin ekle
            var aktifDurum = await context.DURUM_TANIMLARIs
                .FirstOrDefaultAsync(d => d.tablo_adi == "KULLANICILAR" && d.deger_kodu == "AKTIF");

            if (aktifDurum != null)
            {
                var adminKullanici = new KULLANICILAR
                {
                    ad = "Admin",
                    soyad = "User",
                    eposta = "admin@sigorta.com",
                    telefon = "05551234567",
                    durum_id = aktifDurum.id,
                    email_dogrulandi = true,
                    telefon_dogrulandi = true,
                    sifre_hash = "IDENTITY_MANAGED",
                    kayit_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now
                };

                var existingAdmin = await context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == adminKullanici.eposta);

                if (existingAdmin == null)
                {
                    context.KULLANICILARs.Add(adminKullanici);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Admin kullanıcısı KULLANICILAR tablosuna eklendi.");
                }
            }
        }
    }
} 