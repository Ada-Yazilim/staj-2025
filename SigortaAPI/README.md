# Sigorta API — Swagger Yetkilendirme (TR)

1. `POST /api/Auth/login` ile giriş yapın.
2. Dönen `token` değerini kopyalayın.
3. Swagger sağ üstte **Authorize** → Değer: `Bearer <boşluk><token>`
4. Artık yetkili endpointleri deneyebilirsiniz.

## Sık Karşılaşılan Hatalar

- 403 Forbidden: Yetkisiz rol veya eksik Bearer token.
- 401 Unauthorized: Token yok/sonlanmış/geçersiz.
- 400 Bad Request: İstek gövdesi hatalı.

## Roller

- Customer: Temel kullanıcı
- Agent: Müşteri yönetimi
- Admin: Tam yetki

## Opsiyonel Sağlık Kontrolü

“/health endpoint’i yoksa ileride eklenebilir (Microsoft.Extensions.Diagnostics.HealthChecks).”