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
    public class PoliceTeklifleriController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public PoliceTeklifleriController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/PoliceTeklifleri
        [HttpGet]
        public async Task<IActionResult> GetPoliceTeklifleri([FromQuery] int? musteriId = null, [FromQuery] string? durum = null)
        {
            try
            {
                var query = _context.POLICE_TEKLIFLERIs
                    .Include(t => t.musteri)
                    .Include(t => t.police_turu)
                    .Include(t => t.sigorta_sirketi)
                    .Include(t => t.olusturan_kullanici)
                    .Include(t => t.durum)
                    .AsQueryable();

                if (musteriId.HasValue)
                    query = query.Where(t => t.musteri_id == musteriId.Value);

                if (!string.IsNullOrEmpty(durum))
                    query = query.Where(t => t.durum.deger_kodu == durum);

                var teklifler = await query
                    .OrderByDescending(t => t.teklif_tarihi)
                    .Select(t => new PoliceTeklifListDto
                    {
                        id = t.id,
                        teklif_no = t.teklif_no,
                        musteri_adi = ($"{t.musteri.ad} {t.musteri.soyad}").Trim(),
                        police_turu_adi = t.police_turu.urun_adi,
                        sigorta_sirketi_adi = t.sigorta_sirketi.sirket_adi,
                        brut_prim = t.brut_prim,
                        net_prim = t.net_prim,
                        toplam_tutar = t.toplam_tutar,
                        durum_adi = t.durum.deger_aciklama,
                        teklif_tarihi = t.teklif_tarihi,
                        gecerlilik_tarihi = t.gecerlilik_tarihi,
                        olusturan_kullanici = $"{t.olusturan_kullanici.ad} {t.olusturan_kullanici.soyad}"
                    })
                    .ToListAsync();

                return Ok(teklifler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe teklifleri listelenirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/PoliceTeklifleri/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoliceTeklifi(int id)
        {
            try
            {
                var teklif = await _context.POLICE_TEKLIFLERIs
                    .Include(t => t.musteri)
                    .Include(t => t.police_turu)
                    .Include(t => t.sigorta_sirketi)
                    .Include(t => t.olusturan_kullanici)
                    .Include(t => t.durum)
                    .FirstOrDefaultAsync(t => t.id == id);

                if (teklif == null)
                {
                    return NotFound("Poliçe teklifi bulunamadı");
                }

                var teklifDetay = new PoliceTeklifDetayDto
                {
                    id = teklif.id,
                    teklif_no = teklif.teklif_no,
                    musteri_id = teklif.musteri_id,
                    musteri_adi = ($"{teklif.musteri.ad} {teklif.musteri.soyad}").Trim(),
                    police_turu_id = teklif.police_turu_id,
                    police_turu_adi = teklif.police_turu.urun_adi,
                    sigorta_sirketi_id = teklif.sigorta_sirketi_id,
                    sigorta_sirketi_adi = teklif.sigorta_sirketi.sirket_adi,
                    olusturan_kullanici_id = teklif.olusturan_kullanici_id,
                    olusturan_kullanici = $"{teklif.olusturan_kullanici.ad} {teklif.olusturan_kullanici.soyad}",
                    risk_bilgileri = teklif.risk_bilgileri,
                    teminat_bilgileri = teklif.teminat_bilgileri,
                    brut_prim = teklif.brut_prim,
                    net_prim = teklif.net_prim,
                    komisyon_tutari = teklif.komisyon_tutari,
                    vergi_tutari = teklif.vergi_tutari,
                    toplam_tutar = teklif.toplam_tutar,
                    durum_id = teklif.durum_id,
                    durum_adi = teklif.durum.deger_aciklama,
                    teklif_tarihi = teklif.teklif_tarihi,
                    gecerlilik_tarihi = teklif.gecerlilik_tarihi,
                    onay_tarihi = teklif.onay_tarihi,
                    onaylayan_kullanici = teklif.onaylayan_kullanici,
                    red_nedeni = teklif.red_nedeni,
                    notlar = teklif.notlar,
                    olusturma_tarihi = teklif.olusturma_tarihi,
                    guncelleme_tarihi = teklif.guncelleme_tarihi
                };

                return Ok(teklifDetay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe teklifi detayı alınırken hata oluştu: {ex.Message}");
            }
        }

        // POST: api/PoliceTeklifleri
        [HttpPost]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> CreatePoliceTeklifi([FromBody] PoliceTeklifCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Müşteri kontrolü
                var musteri = await _context.MUSTERILERs.FindAsync(createDto.musteri_id);
                if (musteri == null)
                {
                    return BadRequest("Müşteri bulunamadı");
                }

                // Poliçe türü kontrolü
                var policeTuru = await _context.POLICE_TURLERIs.FindAsync(createDto.police_turu_id);
                if (policeTuru == null)
                {
                    return BadRequest("Poliçe türü bulunamadı");
                }

                // Sigorta şirketi kontrolü
                var sigortaSirketi = await _context.SIGORTA_SIRKETLERIs.FindAsync(createDto.sigorta_sirketi_id);
                if (sigortaSirketi == null)
                {
                    return BadRequest("Sigorta şirketi bulunamadı");
                }

                // Bekleyen durum ID'si
                var bekleyenDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "POLICE_TEKLIFLERI" && d.deger_kodu == "BEKLEMEDE")?.id ?? 1;

                // Teklif numarası oluştur
                var teklifNo = await GenerateTeklifNo();

                // Kullanıcı ID'sini al
                var kullaniciId = int.Parse(User.FindFirst("KullanicilarId")?.Value ?? "1");

                var teklif = new POLICE_TEKLIFLERI
                {
                    teklif_no = teklifNo,
                    musteri_id = createDto.musteri_id,
                    police_turu_id = createDto.police_turu_id,
                    sigorta_sirketi_id = createDto.sigorta_sirketi_id,
                    olusturan_kullanici_id = kullaniciId,
                    risk_bilgileri = createDto.risk_bilgileri,
                    teminat_bilgileri = createDto.teminat_bilgileri,
                    brut_prim = createDto.brut_prim,
                    net_prim = createDto.net_prim,
                    komisyon_tutari = createDto.komisyon_tutari,
                    vergi_tutari = createDto.vergi_tutari,
                    toplam_tutar = createDto.toplam_tutar,
                    durum_id = bekleyenDurumId,
                    teklif_tarihi = DateTime.Now,
                    gecerlilik_tarihi = DateTime.Now.AddDays(30), // 30 gün geçerli
                    notlar = createDto.notlar,
                    olusturma_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now
                };

                _context.POLICE_TEKLIFLERIs.Add(teklif);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPoliceTeklifi), new { id = teklif.id }, 
                    new { id = teklif.id, teklif_no = teklif.teklif_no });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe teklifi oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/PoliceTeklifleri/5/onayla
        [HttpPut("{id}/onayla")]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> OnaylaTeklif(int id)
        {
            try
            {
                var teklif = await _context.POLICE_TEKLIFLERIs
                    .Include(t => t.musteri)
                    .Include(t => t.police_turu)
                    .Include(t => t.sigorta_sirketi)
                    .FirstOrDefaultAsync(t => t.id == id);

                if (teklif == null)
                {
                    return NotFound("Poliçe teklifi bulunamadı");
                }

                // Onaylanmış durum ID'si
                var onaylanmisDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "POLICE_TEKLIFLERI" && d.deger_kodu == "ONAYLANDI")?.id ?? 2;

                teklif.durum_id = onaylanmisDurumId;
                teklif.onay_tarihi = DateTime.Now;
                teklif.onaylayan_kullanici = User.Identity?.Name ?? "System";
                teklif.guncelleme_tarihi = DateTime.Now;

                await _context.SaveChangesAsync();

                // Müşteriye bildirim gönder
                await SendNotificationToCustomer(teklif.musteri_id, 
                    $"Poliçe teklifiniz onaylandı! Teklif No: {teklif.teklif_no}");

                return Ok(new { 
                    message = "Poliçe teklifi onaylandı", 
                    teklif_no = teklif.teklif_no,
                    musteri_adi = ($"{teklif.musteri.ad} {teklif.musteri.soyad}").Trim(),
                    police_turu = teklif.police_turu.urun_adi,
                    sigorta_sirketi = teklif.sigorta_sirketi.sirket_adi,
                    toplam_tutar = teklif.toplam_tutar
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe teklifi onaylanırken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/PoliceTeklifleri/5/reddet
        [HttpPut("{id}/reddet")]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> ReddetTeklif(int id, [FromBody] string redNedeni)
        {
            try
            {
                var teklif = await _context.POLICE_TEKLIFLERIs.FindAsync(id);
                if (teklif == null)
                {
                    return NotFound("Poliçe teklifi bulunamadı");
                }

                // Reddedildi durum ID'si
                var reddedildiDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "POLICE_TEKLIFLERI" && d.deger_kodu == "REDDEDILDI")?.id ?? 3;

                teklif.durum_id = reddedildiDurumId;
                teklif.red_nedeni = redNedeni;
                teklif.guncelleme_tarihi = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Poliçe teklifi reddedildi", teklif_no = teklif.teklif_no });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe teklifi reddedilirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/PoliceTeklifleri/lookup-data
        [HttpGet("lookup-data")]
        public async Task<IActionResult> GetLookupData()
        {
            try
            {
                var policeTurleri = await _context.POLICE_TURLERIs
                    .Where(p => p.aktif_mi)
                    .OrderBy(p => p.urun_adi)
                    .Select(p => new { id = p.id, text = p.urun_adi })
                    .ToListAsync();

                var sigortaSirketleri = await _context.SIGORTA_SIRKETLERIs
                    .Where(s => s.aktif_mi)
                    .OrderBy(s => s.sirket_adi)
                    .Select(s => new { id = s.id, text = s.sirket_adi })
                    .ToListAsync();

                var durumlar = await _context.DURUM_TANIMLARIs
                    .Where(d => d.tablo_adi == "POLICE_TEKLIFLERI" && d.aktif_mi)
                    .OrderBy(d => d.siralama)
                    .Select(d => new { id = d.id, text = d.deger_aciklama, kod = d.deger_kodu })
                    .ToListAsync();

                return Ok(new
                {
                    police_turleri = policeTurleri,
                    sigorta_sirketleri = sigortaSirketleri,
                    durumlar = durumlar
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lookup verileri alınırken hata oluştu: {ex.Message}");
            }
        }

        private async Task<string> GenerateTeklifNo()
        {
            var yil = DateTime.Now.Year.ToString();
            var prefix = $"TEK{yil}";
            
            var sonNo = await _context.POLICE_TEKLIFLERIs
                .Where(t => t.teklif_no.StartsWith(prefix))
                .Select(t => t.teklif_no)
                .OrderByDescending(t => t)
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

            return $"{prefix}{siradakiNo:D6}"; // TEK2024000001 formatı
        }

        private async Task SendNotificationToCustomer(int musteriId, string message)
        {
            try
            {
                var musteri = await _context.MUSTERILERs
                    .Include(m => m.kullanici)
                    .FirstOrDefaultAsync(m => m.id == musteriId);

                if (musteri?.kullanici != null)
                {
                    var bildirim = new BILDIRIMLER
                    {
                        alici_kullanici_id = musteri.kullanici.id,
                        baslik = "Poliçe Teklifi Güncellemesi",
                        icerik = message,
                        gonderim_tarihi = DateTime.Now,
                        okundu_mu = false
                    };

                    _context.BILDIRIMLERs.Add(bildirim);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Bildirim gönderilemese bile ana işlem devam etsin
                Console.WriteLine($"Bildirim gönderilemedi: {ex.Message}");
            }
        }
    }
} 