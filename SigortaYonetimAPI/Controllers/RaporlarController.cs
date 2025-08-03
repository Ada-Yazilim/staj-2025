using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;
using SigortaYonetimAPI.Models.DTOs;
using System.Security.Claims;

namespace SigortaYonetimAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,ACENTE")]
    public class RaporlarController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public RaporlarController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // Genel istatistikler
        [HttpGet("genel-istatistikler")]
        public async Task<ActionResult<GenelIstatistiklerDto>> GetGenelIstatistikler()
        {
            var bugun = DateTime.Today;
            var buAy = new DateTime(bugun.Year, bugun.Month, 1);
            var buYil = new DateTime(bugun.Year, 1, 1);

            var istatistikler = new GenelIstatistiklerDto
            {
                ToplamMusteri = await _context.MUSTERILERs.CountAsync(),
                ToplamPolice = await _context.POLISELERs.CountAsync(),
                ToplamHasar = await _context.HASAR_DOSYALARs.CountAsync(),
                ToplamOdeme = await _context.ODEMELERs.CountAsync(),
                
                BuAyYeniMusteri = await _context.MUSTERILERs
                    .Where(m => m.kayit_tarihi >= buAy).CountAsync(),
                BuAyYeniPolice = await _context.POLISELERs
                    .Where(p => p.tanzim_tarihi >= buAy).CountAsync(),
                BuAyYeniHasar = await _context.HASAR_DOSYALARs
                    .Where(h => h.olusturma_tarihi >= buAy).CountAsync(),
                BuAyToplamOdeme = await _context.ODEMELERs
                    .Where(o => o.odeme_tarihi >= buAy && o.durum_id == 2).SumAsync(o => o.tutar),
                
                BuYilToplamPrim = await _context.POLISELERs
                    .Where(p => p.tanzim_tarihi >= buYil).SumAsync(p => p.brut_prim ?? 0),
                BuYilToplamHasarTutari = await _context.HASAR_DOSYALARs
                    .Where(h => h.olusturma_tarihi >= buYil && h.durum_id == 4).SumAsync(h => h.toplam_tutar ?? 0)
            };

            return Ok(istatistikler);
        }

        // Poliçe türlerine göre dağılım
        [HttpGet("police-turleri-dagilimi")]
        public async Task<ActionResult<IEnumerable<PoliceTurDagilimiDto>>> GetPoliceTurDagilimi()
        {
            var dagilim = await _context.POLISELERs
                .Include(p => p.police_turu)
                .GroupBy(p => new { p.police_turu_id, p.police_turu.urun_adi })
                .Select(g => new PoliceTurDagilimiDto
                {
                    PoliceTuruAdi = g.Key.urun_adi,
                    Adet = g.Count(),
                    ToplamPrim = g.Sum(p => p.brut_prim ?? 0)
                })
                .OrderByDescending(x => x.Adet)
                .ToListAsync();

            return Ok(dagilim);
        }

        // Sigorta şirketlerine göre dağılım
        [HttpGet("sirket-dagilimi")]
        public async Task<ActionResult<IEnumerable<SirketDagilimiDto>>> GetSirketDagilimi()
        {
            var dagilim = await _context.POLISELERs
                .Include(p => p.sigorta_sirketi)
                .GroupBy(p => new { p.sigorta_sirketi_id, p.sigorta_sirketi.sirket_adi })
                .Select(g => new SirketDagilimiDto
                {
                    SirketAdi = g.Key.sirket_adi,
                    Adet = g.Count(),
                    ToplamPrim = g.Sum(p => p.brut_prim ?? 0)
                })
                .OrderByDescending(x => x.ToplamPrim)
                .ToListAsync();

            return Ok(dagilim);
        }

        // Aylık poliçe oluşturma trendi
        [HttpGet("aylik-police-trendi")]
        public async Task<ActionResult<IEnumerable<AylikTrendDto>>> GetAylikPoliceTrendi([FromQuery] int yil = 0)
        {
            if (yil == 0) yil = DateTime.Now.Year;

            var trend = await _context.POLISELERs
                .Where(p => p.tanzim_tarihi.Year == yil)
                .GroupBy(p => p.tanzim_tarihi.Month)
                .Select(g => new AylikTrendDto
                {
                    Ay = g.Key,
                    AyAdi = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Adet = g.Count(),
                    ToplamPrim = g.Sum(p => p.brut_prim ?? 0)
                })
                .OrderBy(x => x.Ay)
                .ToListAsync();

            return Ok(trend);
        }

        // Hasar durumlarına göre dağılım
        [HttpGet("hasar-durum-dagilimi")]
        public async Task<ActionResult<IEnumerable<HasarDurumDagilimiDto>>> GetHasarDurumDagilimi()
        {
            var dagilim = await _context.HASAR_DOSYALARs
                .Include(h => h.durum)
                .GroupBy(h => new { h.durum_id, h.durum.deger_aciklama })
                .Select(g => new HasarDurumDagilimiDto
                {
                    DurumAdi = g.Key.deger_aciklama,
                    Adet = g.Count(),
                    ToplamTutar = g.Sum(h => h.toplam_tutar ?? 0)
                })
                .OrderByDescending(x => x.Adet)
                .ToListAsync();

            return Ok(dagilim);
        }

        // Ödeme durumlarına göre dağılım
        [HttpGet("odeme-durum-dagilimi")]
        public async Task<ActionResult<IEnumerable<OdemeDurumDagilimiDto>>> GetOdemeDurumDagilimi()
        {
            var dagilim = await _context.ODEMELERs
                .Include(o => o.durum)
                .GroupBy(o => new { o.durum_id, o.durum.deger_aciklama })
                .Select(g => new OdemeDurumDagilimiDto
                {
                    DurumAdi = g.Key.deger_aciklama,
                    Adet = g.Count(),
                    ToplamTutar = g.Sum(o => o.tutar)
                })
                .OrderByDescending(x => x.Adet)
                .ToListAsync();

            return Ok(dagilim);
        }

        // En çok hasar bildiren müşteriler
        [HttpGet("en-cok-hasar-musteriler")]
        public async Task<ActionResult<IEnumerable<EnCokHasarMusteriDto>>> GetEnCokHasarMusteriler([FromQuery] int limit = 10)
        {
            var musteriler = await _context.HASAR_DOSYALARs
                .Include(h => h.musteri)
                .GroupBy(h => new { h.musteri_id, h.musteri.ad, h.musteri.soyad })
                .Select(g => new EnCokHasarMusteriDto
                {
                    MusteriAdi = ($"{g.Key.ad} {g.Key.soyad}").Trim(),
                    HasarSayisi = g.Count(),
                    ToplamTutar = g.Sum(h => h.toplam_tutar ?? 0)
                })
                .OrderByDescending(x => x.HasarSayisi)
                .Take(limit)
                .ToListAsync();

            return Ok(musteriler);
        }

        // En yüksek primli poliçeler
        [HttpGet("en-yuksek-prim-policeler")]
        public async Task<ActionResult<IEnumerable<EnYuksekPrimPoliceDto>>> GetEnYuksekPrimPoliceler([FromQuery] int limit = 10)
        {
            var policeler = await _context.POLISELERs
                .Include(p => p.musteri)
                .Include(p => p.police_turu)
                .OrderByDescending(p => p.brut_prim ?? 0)
                .Take(limit)
                .Select(p => new EnYuksekPrimPoliceDto
                {
                    PoliceNo = p.police_no,
                    MusteriAdi = ($"{p.musteri.ad} {p.musteri.soyad}").Trim(),
                    PoliceTuruAdi = p.police_turu.urun_adi,
                    BrutPrim = p.brut_prim ?? 0,
                    OlusturmaTarihi = p.tanzim_tarihi
                })
                .ToListAsync();

            return Ok(policeler);
        }

        // Komisyon raporu
        [HttpGet("komisyon-raporu")]
        public async Task<ActionResult<KomisyonRaporuDto>> GetKomisyonRaporu([FromQuery] DateTime? baslangic = null, [FromQuery] DateTime? bitis = null)
        {
            var query = _context.POLISELERs.AsQueryable();

            if (baslangic.HasValue)
                query = query.Where(p => p.tanzim_tarihi >= baslangic.Value);
            if (bitis.HasValue)
                query = query.Where(p => p.tanzim_tarihi <= bitis.Value);

            var rapor = new KomisyonRaporuDto
            {
                ToplamKomisyon = await query.SumAsync(p => p.komisyon_tutari ?? 0),
                ToplamPrim = await query.SumAsync(p => p.brut_prim ?? 0),
                PoliceSayisi = await query.CountAsync(),
                OrtalamaKomisyonOrani = await query.AverageAsync(p => (p.komisyon_tutari ?? 0) / (p.brut_prim ?? 1) * 100)
            };

            return Ok(rapor);
        }
    }
} 