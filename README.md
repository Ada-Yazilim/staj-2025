# Sigortacılık Uygulaması (Staj 2025)

Bu proje; ASP.NET Core Web API (.NET 8) + React (Vite) tabanlı bir sigortacılık örnek uygulamasıdır.
Kimlik doğrulama **Identity + JWT**, yetkilendirme **Rol** bazlıdır.

## Hızlı Başlangıç

### Gereksinimler
- .NET 8 SDK
- Node.js 20+
- SQL Server (veya localdb)

### Backend (API)
```bash
cd SigortaAPI/SigortaAPI
# Geliştirme ayar örneğini kopyalayın ve değerleri doldurun
# appsettings.Development.json.example -> appsettings.Development.json
dotnet restore
dotnet ef database update   # migration uygula
dotnet run
```

* Swagger (Geliştirme): `https://localhost:****/swagger`
* İlk çalıştırmada **Admin** rolü ve `admin / Admin123!` kullanıcısı seed edilir.

### Frontend

```bash
cd frontend
# .env.example -> .env (VITE_API_URL değerini girin, ör: https://localhost:44305)
npm install
npm run dev
```

### CORS

Varsayılan izin verilen kaynaklar: `http://localhost:5173`, `http://localhost:5174`, `http://localhost:3000`.
Gerekirse `Program.cs` içindeki `AllowFrontend` politikasını güncelleyin.

## Kimlik Doğrulama / Yetkilendirme

1. **Giriş**: `POST /api/Auth/login`
2. Dönüş:

```json
{
  "token": "JWT_TOKEN",
  "expiration": "yyyy-mm-ddThh:mm:ssZ",
  "roles": ["Admin","Agent"]
}
```

3. Swagger’da **Authorize** → `Bearer JWT_TOKEN`
4. Frontend `localStorage.token` üzerinden rolleri çözümler.

## Önemli Endpointler

* `GET /api/customers` → **Agent/Admin**
* `GET /api/customers/{id}` → **Agent/Admin**
* `POST/PUT /api/customers` → **Agent/Admin**
* `DELETE /api/customers/{id}` → **Admin**

> Policies/Claims/Payments/Documents için benzer desen geçerlidir.

## Geliştirme Komutları

* Migration ekleme: `dotnet ef migrations add <Ad>`
* Migration geri alma: `dotnet ef database update <MigrationAdı>`

## Katkı & Git Akışı (Fork)

* Senkron:

```bash
git remote add upstream <ORİJİNAL_REPO_URL> || true
git fetch upstream
git rebase upstream/main   # veya merge
```

* Push:

```bash
git push origin melih-alcik
```

* PR: Fork’tan orijinal repo **main**’ine PR açın. Açıklamayı Türkçe yazın.

## Lisans

Bu proje eğitim amaçlıdır.

## Opsiyonel Sağlık Kontrolü

“/health endpoint’i yoksa ileride eklenebilir (Microsoft.Extensions.Diagnostics.HealthChecks).”
