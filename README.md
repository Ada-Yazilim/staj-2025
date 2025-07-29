# SigortaAPI

**SigortaAPI**, Ada Yazılım 2025 Staj Programı kapsamında geliştirilmiş, JWT tabanlı kimlik doğrulama ve yetkilendirme desteğine sahip bir ASP.NET Core Web API projesidir.

---

## 📋 İçindekiler

1. [Genel Bakış](#genel-bakış)
2. [Özellikler](#özellikler)
3. [Önkoşullar](#önkoşullar)
4. [Kurulum](#kurulum)
5. [Yapılandırma](#yapılandırma)
6. [Çalıştırma](#çalıştırma)
7. [API Dokümantasyonu](#api-dokümantasyonu)
8. [Authorization Akışı](#authorization-akışı)
9. [Unit Testler](#unit-testler)
10. [Katkıda Bulunma](#katkıda-bulunma)
11. [Lisans](#lisans)

---

## 🎯 Genel Bakış

SigortaAPI, sigorta işlemlerini yönetmek için temel CRUD (Create, Read, Update, Delete) endpoint’leri sunan bir servis katmanıdır. Proje, güvenlik ve esneklik amacıyla JWT (JSON Web Token) tabanlı authentication ve role-based authorization ile entegre edilmiştir.

## 🚀 Özellikler

* ASP.NET Core 8.0 Web API
* Entity Framework Core ile SQL Server veri erişimi
* JWT Access & Refresh token üretimi ve yenileme
* Role-based authorization (Admin, User)
* Swagger UI ile interaktif API testi
* Konsistente hata yönetimi (standardize JSON response)
* Unit test altyapısı

## 🛠️ Önkoşullar

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server (LocalDB veya tam sürüm)
* Visual Studio 2022 / VS Code
* (Opsiyonel) Postman veya benzeri API test aracı

## ⚙️ Kurulum

1. Depoyu klonlayın:

   ```bash
   git clone https://github.com/kullaniciAdin/staj-2025.git
   cd staj-2025/SigortaAPI
   ```
2. Bağımlılıkları yükleyin:

   ```bash
   dotnet restore
   ```
3. Veritabanı bağlantısını yapılandırın (bkz. [Yapılandırma](#yapılandırma)).

## 🔧 Yapılandırma

`appsettings.json` içinde aşağıdaki placeholder alanları doldurun:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<YOUR_SQL_SERVER_CONNECTION_STRING>"
  },
  "JwtSettings": {
    "Key": "<YOUR_SECRET_KEY>",
    "Issuer": "SigortaAPI",
    "Audience": "SigortaClient",
    "AccessTokenDurationMinutes": 15,
    "RefreshTokenDurationDays": 7
  }
}
```

## ▶️ Çalıştırma

Proje klasöründe:

```bash
dotnet build
dotnet run
```

Tarayıcıda `https://localhost:<PORT>/swagger` adresine giderek Swagger UI üzerinden API’yi keşfedebilirsiniz.

## 📑 API Dokümantasyonu

**Kullanılabilir Endpoint’ler:**

| Method | Endpoint              | Açıklama                          |
| ------ | --------------------- | --------------------------------- |
| POST   | `/api/auth/login`     | Kullanıcı girişi ve token üretimi |
| POST   | `/api/auth/refresh`   | Refresh token ile token yenileme  |
| POST   | `/api/auth/register`  | Yeni kullanıcı kaydı              |
| GET    | `/api/customers`      | Tüm müşterileri listele           |
| GET    | `/api/customers/{id}` | Belirli müşteri bilgisini getir   |
| POST   | `/api/customers`      | Yeni müşteri oluştur              |
| PUT    | `/api/customers/{id}` | Müşteri güncelleme                |
| DELETE | `/api/customers/{id}` | Müşteri silme                     |

Daha ayrıntılı dokümantasyon için placeholder:

* Örnek istek/giriş payload’ları
* Örnek cevap yapıları

## 🔐 Authorization Akışı

1. **Token Üretimi:**

   * `POST /api/auth/login` ile Access ve Refresh token’lar oluşturulur.
2. **Token Doğrulama:**

   * Her istekte HTTP Header’da `Authorization: Bearer <token>` kullanılır.
3. **Token Yenileme:**

   * `POST /api/auth/refresh` ile geçerli Refresh token sunulur, yeni token çifti alınır.
4. **Role Kontrolü:**
   -roller `[Authorize(Policy = "AdminOnly")]` gibi attribute’larla kontrol edilir.

## 🧪 Unit Testler

> **Placeholder:** Unit test projeleri ve test senaryolarının listesi burada yer alacak.

## 🤝 Katkıda Bulunma

> **Placeholder:** Projeye katkı yapma rehberi, PR süreci, kod stili vb.

## 📜 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakınız.
