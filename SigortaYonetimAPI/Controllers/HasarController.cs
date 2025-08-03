using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;
using SigortaYonetimAPI.Models.DTOs;

namespace SigortaYonetimAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HasarController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public HasarController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/Hasar
        [HttpGet]
        public async Task<IActionResult> GetHasarDosyalari([FromQuery] int? musteriId = null, [FromQuery] int? policeId = null, [FromQuery] int? kullanici_id = null)
        {
            var query = _context.HASAR_DOSYALARs
                .Include(h => h.police)
                .Include(h => h.musteri)
                .Include(h => h.durum)
                .AsQueryable();

            if (musteriId.HasValue)
                query = query.Where(h => h.musteri_id == musteriId.Value);
            if (policeId.HasValue)
                query = query.Where(h => h.police_id == policeId.Value);

            // KULLANICI rolü için sadece kendi hasarlarını göster
            if (kullanici_id.HasValue)
            {
                query = query.Where(h => h.musteri_id == kullanici_id.Value);
            }

            var hasarListRaw = await query
                .OrderByDescending(h => h.olusturma_tarihi)
                .Select(h => new {
                    h.id,
                    h.dosya_no,
                    police_no = h.police.police_no,
                    musteri = h.musteri,
                    durum_adi = h.durum.deger_aciklama,
                    h.olusturma_tarihi,
                    h.toplam_tutar
                })
                .ToListAsync();

            var hasarlar = hasarListRaw.Select(h => new HasarListDto
            {
                id = h.id,
                dosya_no = h.dosya_no ?? string.Empty,
                police_no = h.police_no ?? string.Empty,
                musteri_adi = h.musteri != null ?
                    ($"{h.musteri.ad} {h.musteri.soyad}").Trim()
                    : "",
                durum_adi = h.durum_adi ?? string.Empty,
                olusturma_tarihi = h.olusturma_tarihi,
                toplam_tutar = h.toplam_tutar
            }).ToList();

            return Ok(hasarlar);
        }

        // GET: api/Hasar/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHasarDetay(int id)
        {
            var hasar = await _context.HASAR_DOSYALARs
                .Include(h => h.police)
                .Include(h => h.musteri)
                .Include(h => h.durum)
                .FirstOrDefaultAsync(h => h.id == id);
            if (hasar == null) return NotFound("Hasar dosyası bulunamadı");

            var detay = new HasarDetayDto
            {
                id = hasar.id,
                dosya_no = hasar.dosya_no,
                police_id = hasar.police_id,
                police_no = hasar.police.police_no,
                musteri_id = hasar.musteri_id,
                musteri_adi = hasar.musteri?.ad ?? string.Join(" ", new[] { hasar.musteri?.ad, hasar.musteri?.soyad }.Where(x => !string.IsNullOrEmpty(x))),
                durum_id = hasar.durum_id,
                durum_adi = hasar.durum.deger_aciklama,
                aciklama = hasar.aciklama,
                toplam_tutar = hasar.toplam_tutar,
                olusturma_tarihi = hasar.olusturma_tarihi,
                guncelleme_tarihi = hasar.guncelleme_tarihi
            };
            return Ok(detay);
        }

        // POST: api/Hasar
        [HttpPost]
        public async Task<IActionResult> CreateHasar([FromBody] HasarCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.toplam_tutar.HasValue && dto.toplam_tutar < 0)
                return BadRequest("Toplam tutar negatif olamaz.");
            if (!string.IsNullOrEmpty(dto.aciklama) && dto.aciklama.Length > 500)
                return BadRequest("Açıklama en fazla 500 karakter olabilir.");

            var police = await _context.POLISELERs.FindAsync(dto.police_id);
            if (police == null) return BadRequest("Poliçe bulunamadı");

            var musteri = await _context.MUSTERILERs.FindAsync(dto.musteri_id);
            if (musteri == null) return BadRequest("Müşteri bulunamadı");

            var bekleyenDurumId = _context.DURUM_TANIMLARIs.FirstOrDefault(d => d.tablo_adi == "HASAR_DOSYALAR" && d.deger_kodu == "BEKLEMEDE")?.id ?? 1;
            var dosyaNo = await GenerateDosyaNo();

            var hasar = new HASAR_DOSYALAR
            {
                dosya_no = dosyaNo,
                police_id = dto.police_id,
                musteri_id = dto.musteri_id,
                durum_id = bekleyenDurumId,
                aciklama = dto.aciklama,
                toplam_tutar = dto.toplam_tutar,
                olusturma_tarihi = DateTime.Now,
                guncelleme_tarihi = DateTime.Now
            };
            _context.HASAR_DOSYALARs.Add(hasar);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHasarDetay), new { id = hasar.id }, new { id = hasar.id, dosya_no = hasar.dosya_no });
        }

        // PUT: api/Hasar/5/durum
        [HttpPut("{id}/durum")]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> UpdateHasarDurum(int id, [FromBody] HasarDurumUpdateDto dto)
        {
            var hasar = await _context.HASAR_DOSYALARs.FindAsync(id);
            if (hasar == null) return NotFound("Hasar dosyası bulunamadı");
            hasar.durum_id = dto.durum_id;
            hasar.guncelleme_tarihi = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Hasar durumu güncellendi" });
        }

        private async Task<string> GenerateDosyaNo()
        {
            var yil = DateTime.Now.Year.ToString();
            var prefix = $"HSR{yil}";
            var sonNo = await _context.HASAR_DOSYALARs
                .Where(h => h.dosya_no.StartsWith(prefix))
                .Select(h => h.dosya_no)
                .OrderByDescending(h => h)
                .FirstOrDefaultAsync();
            int siradakiNo = 1;
            if (!string.IsNullOrEmpty(sonNo))
            {
                var noKismi = sonNo.Substring(prefix.Length);
                if (int.TryParse(noKismi, out int mevcutNo))
                {
                    siradakiNo = mevcutNo + 1;
                }
            }
            return $"{prefix}{siradakiNo:D6}";
        }
    }
} 