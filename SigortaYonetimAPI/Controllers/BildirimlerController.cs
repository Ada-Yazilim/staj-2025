using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;

namespace SigortaYonetimAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BildirimlerController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public BildirimlerController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/Bildirimler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BildirimDto>>> GetBildirimler()
        {
            try
            {
                // Kullanıcı ID'sini al
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı");
                }

                var kullanici = await _context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == userEmail);

                if (kullanici == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var bildirimler = await _context.BILDIRIMLERs
                    .Where(b => b.alici_kullanici_id == kullanici.id)
                    .OrderByDescending(b => b.gonderim_tarihi)
                    .Select(b => new BildirimDto
                    {
                        id = b.id,
                        alici_kullanici_id = b.alici_kullanici_id,
                        baslik = b.baslik,
                        icerik = b.icerik,
                        gonderim_tarihi = b.gonderim_tarihi.ToString("yyyy-MM-ddTHH:mm:ss"),
                        okundu_mu = b.okundu_mu ?? false
                    })
                    .ToListAsync();

                return Ok(bildirimler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bildirimler alınırken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Bildirimler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BildirimDto>> GetBildirim(int id)
        {
            try
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı");
                }

                var kullanici = await _context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == userEmail);

                if (kullanici == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var bildirim = await _context.BILDIRIMLERs
                    .Where(b => b.id == id && b.alici_kullanici_id == kullanici.id)
                    .Select(b => new BildirimDto
                    {
                        id = b.id,
                        alici_kullanici_id = b.alici_kullanici_id,
                        baslik = b.baslik,
                        icerik = b.icerik,
                        gonderim_tarihi = b.gonderim_tarihi.ToString("yyyy-MM-ddTHH:mm:ss"),
                        okundu_mu = b.okundu_mu ?? false
                    })
                    .FirstOrDefaultAsync();

                if (bildirim == null)
                {
                    return NotFound("Bildirim bulunamadı");
                }

                return Ok(bildirim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bildirim alınırken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/Bildirimler/5/okundu
        [HttpPut("{id}/okundu")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı");
                }

                var kullanici = await _context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == userEmail);

                if (kullanici == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var bildirim = await _context.BILDIRIMLERs
                    .FirstOrDefaultAsync(b => b.id == id && b.alici_kullanici_id == kullanici.id);

                if (bildirim == null)
                {
                    return NotFound("Bildirim bulunamadı");
                }

                bildirim.okundu_mu = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Bildirim okundu olarak işaretlendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bildirim güncellenirken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/Bildirimler/tumunu-okundu
        [HttpPut("tumunu-okundu")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı");
                }

                var kullanici = await _context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == userEmail);

                if (kullanici == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var okunmamisBildirimler = await _context.BILDIRIMLERs
                    .Where(b => b.alici_kullanici_id == kullanici.id && (b.okundu_mu == null || !b.okundu_mu.Value))
                    .ToListAsync();

                foreach (var bildirim in okunmamisBildirimler)
                {
                    bildirim.okundu_mu = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = $"{okunmamisBildirimler.Count} bildirim okundu olarak işaretlendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bildirimler güncellenirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Bildirimler/okunmamis-sayisi
        [HttpGet("okunmamis-sayisi")]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            try
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı");
                }

                var kullanici = await _context.KULLANICILARs
                    .FirstOrDefaultAsync(k => k.eposta == userEmail);

                if (kullanici == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var okunmamisSayisi = await _context.BILDIRIMLERs
                    .CountAsync(b => b.alici_kullanici_id == kullanici.id && (b.okundu_mu == null || !b.okundu_mu.Value));

                return Ok(okunmamisSayisi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Okunmamış bildirim sayısı alınırken hata oluştu: {ex.Message}");
            }
        }
    }

    public class BildirimDto
    {
        public int id { get; set; }
        public int alici_kullanici_id { get; set; }
        public string baslik { get; set; } = string.Empty;
        public string icerik { get; set; } = string.Empty;
        public string gonderim_tarihi { get; set; } = string.Empty;
        public bool okundu_mu { get; set; }
    }
} 