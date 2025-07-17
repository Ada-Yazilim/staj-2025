# Sigorta Yönetim Platformu

## 📋 Proje Hakkında

Bu proje, **Ada Yazılım 2025 Staj Programı** kapsamında geliştirilmiş modern bir sigorta yönetim platformudur. Proje, sigortacılık sektöründeki temel iş süreçlerini dijital ortama taşıyarak, müşteri, poliçe, hasar ve ödeme yönetimini kolaylaştırmayı amaçlamaktadır.

## 👨‍💻 Geliştirici Bilgileri

- **Geliştirici:** Mert Kan
- **E-posta:** mrttkan@gmail.com
- **Şirket:** Ada Yazılım
- **Proje:** 2025 Staj Programı
- **Başlangıç Tarihi:** Temmuz 2025

## 🛠️ Kullanılan Teknolojiler

### Backend
- **.NET 8** - Modern, cross-platform web framework
- **Entity Framework Core** - ORM (Object-Relational Mapping)
- **SQL Server** - Veritabanı yönetim sistemi
- **ASP.NET Core Web API** - RESTful API geliştirme
- **Swagger/OpenAPI** - API dokümantasyonu

### Frontend
- **React 19.1.0** - Modern JavaScript kütüphanesi
- **TypeScript** - Tip güvenli JavaScript
- **CSS3** - Modern stil ve animasyonlar
- **Fetch API** - HTTP istekleri

### Veritabanı
- **SQL Server 2022** - İlişkisel veritabanı
- **SSMS (SQL Server Management Studio)** - Veritabanı yönetim aracı
- **DB First Yaklaşımı** - Veritabanından kod üretimi

### Geliştirme Araçları
- **Visual Studio** - Kod editörü
- **PowerShell** - Terminal ve komut satırı
- **Git** - Versiyon kontrol sistemi

## 🏗️ Proje Mimarisi

### Backend Mimarisi
```
SigortaYonetimAPI/
├── Controllers/          # API Controller'ları
│   └── KullanicilarController.cs
├── Models/              # Entity Framework Modelleri
│   ├── SigortaYonetimDbContext.cs
│   ├── KULLANICILAR.cs
│   ├── MUSTERILER.cs
│   └── [Diğer modeller...]
├── Program.cs           # Uygulama başlangıç noktası
└── appsettings.json     # Konfigürasyon dosyası
```

### Frontend Mimarisi
```
sigorta-yonetim-frontend/
├── src/
│   ├── App.tsx          # Ana uygulama bileşeni
│   ├── App.css          # Stil dosyası
│   └── index.tsx        # Giriş noktası
├── public/              # Statik dosyalar
└── package.json         # Bağımlılıklar
```

### Veritabanı Mimarisi
```
Database/
├── 01_create_tables.sql    # Tablo oluşturma scripti
└── 02_seed_data.sql        # Örnek veri yükleme scripti
```

## 📊 Veritabanı Şeması

Proje, kapsamlı bir veritabanı şemasına sahiptir:

### Ana Tablolar
- **KULLANICILAR** - Kullanıcı bilgileri ve kimlik doğrulama
- **MUSTERILER** - Bireysel ve kurumsal müşteri bilgileri
- **POLISELER** - Sigorta poliçeleri
- **HASAR_DOSYALAR** - Hasar bildirimleri ve takibi
- **TAKSITLER** - Poliçe taksit bilgileri
- **ODEMELER** - Ödeme kayıtları
- **DURUM_TANIMLARI** - Sistem geneli durum kodları

### İlişkisel Tablolar
- **KULLANICI_ROLLER** - Kullanıcı yetki yönetimi
- **POLICE_TURLERI** - Poliçe türleri ve kategorileri
- **SIGORTA_SIRKETLERI** - Sigorta şirketi bilgileri

## 🚀 Tamamlanan Özellikler

### ✅ Backend (API)
- [x] Entity Framework Core ile DB First yaklaşımı
- [x] RESTful API endpoint'leri
- [x] Kullanıcı listesi API'si
- [x] CORS ayarları (React entegrasyonu için)
- [x] Swagger API dokümantasyonu
- [x] Veritabanı bağlantısı ve model oluşturma

### ✅ Frontend (React)
- [x] Modern React 19 uygulaması
- [x] TypeScript entegrasyonu
- [x] API entegrasyonu (fetch ile)
- [x] Responsive tasarım
- [x] Kullanıcı listesi görüntüleme
- [x] Modern UI/UX tasarımı

### ✅ Veritabanı
- [x] SQL Server veritabanı kurulumu
- [x] Tablo oluşturma scriptleri
- [x] Örnek veri yükleme (seed data)
- [x] İlişkisel veritabanı tasarımı
- [x] DB First kod üretimi

## 📱 Ekran Görüntüleri

### Ana Sayfa
- Modern gradient arka plan
- Kullanıcı kartları grid layout
- Hover efektleri ve animasyonlar
- Responsive tasarım

### API Dokümantasyonu
- Swagger UI ile interaktif API testi
- Endpoint'lerin otomatik dokümantasyonu
- Request/Response örnekleri

## 🔧 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8 SDK
- Node.js 22.x
- SQL Server 2022
- SSMS (SQL Server Management Studio)

### Backend Kurulumu
```bash
cd SigortaYonetimAPI
dotnet restore
dotnet run
```

### Frontend Kurulumu
```bash
cd sigorta-yonetim-frontend
npm install
npm start
```

### Veritabanı Kurulumu
1. SQL Server'da `SigortaYonetimDB` veritabanı oluştur
2. `Database/01_create_tables.sql` çalıştır
3. `Database/02_seed_data.sql` çalıştır

## 🌟 Öne Çıkan Özellikler

### Modern Teknoloji Stack
- En güncel .NET 8 ve React 19 kullanımı
- TypeScript ile tip güvenliği
- Modern CSS özellikleri (Grid, Flexbox, Animations)

### Veritabanı Tasarımı
- Kapsamlı ER diyagramı
- İlişkisel veritabanı tasarımı
- DB First yaklaşımı ile hızlı geliştirme

### API Tasarımı
- RESTful API prensipleri
- Swagger ile otomatik dokümantasyon
- CORS desteği

### UI/UX Tasarımı
- Modern ve responsive tasarım
- Gradient arka planlar
- Hover efektleri ve animasyonlar
- Kullanıcı dostu arayüz

## 📈 Gelecek Planları

### Kısa Vadeli (1-2 Hafta)
- [ ] Kullanıcı girişi/çıkışı sistemi
- [ ] JWT token authentication
- [ ] Müşteri yönetimi modülü
- [ ] Poliçe işlemleri

### Orta Vadeli (1 Ay)
- [ ] Hasar yönetimi sistemi
- [ ] Ödeme sistemi entegrasyonu
- [ ] Raporlama modülü
- [ ] Doküman yönetimi

### Uzun Vadeli (2-3 Ay)
- [ ] Mobil uygulama
- [ ] SMS/E-posta entegrasyonu
- [ ] Gelişmiş raporlama
- [ ] Yapay zeka entegrasyonu

## 🤝 Katkıda Bulunanlar

- **Mert Kan** - Full Stack Developer
  - Backend geliştirme (.NET 8, Entity Framework)
  - Frontend geliştirme (React 19, TypeScript)
  - Veritabanı tasarımı ve kurulumu
  - API tasarımı ve entegrasyonu

## 📞 İletişim

- **Geliştirici:** Mert Kan
- **E-posta:** mrttkan@gmail.com
- **Şirket:** Ada Yazılım
- **Proje:** 2025 Staj Programı

## 📄 Lisans

Bu proje Ada Yazılım 2025 Staj Programı kapsamında geliştirilmiştir.

---

**© 2025 Ada Yazılım. Tüm hakları saklıdır.**
