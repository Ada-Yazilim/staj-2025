using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;

namespace SigortaYonetimAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Tüm endpointler kimlik doğrulama gerektirir
    public class KullanicilarController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public KullanicilarController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // Tüm kullanıcıları listele (ADMIN yetkisi gerekli)
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<KULLANICILAR>>> GetKullanicilar()
        {
            return await _context.KULLANICILARs
                .Include(k => k.durum)
                .ToListAsync();
        }

        // Kullanıcı detayı getir (ADMIN yetkisi gerekli)
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<KULLANICILAR>> GetKullanici(int id)
        {
            var kullanici = await _context.KULLANICILARs
                .Include(k => k.durum)
                .FirstOrDefaultAsync(k => k.id == id);

            if (kullanici == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            return kullanici;
        }

        // Kullanıcı güncelle (ADMIN yetkisi gerekli)
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateKullanici(int id, KULLANICILAR kullanici)
        {
            if (id != kullanici.id)
            {
                return BadRequest(new { message = "ID uyuşmazlığı." });
            }

            var mevcutKullanici = await _context.KULLANICILARs.FindAsync(id);
            if (mevcutKullanici == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Güncelleme
            mevcutKullanici.ad = kullanici.ad;
            mevcutKullanici.soyad = kullanici.soyad;
            mevcutKullanici.eposta = kullanici.eposta;
            mevcutKullanici.telefon = kullanici.telefon;
            mevcutKullanici.durum_id = kullanici.durum_id;
            mevcutKullanici.guncelleme_tarihi = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Kullanıcı başarıyla güncellendi." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Güncelleme sırasında hata oluştu." });
            }
        }

        // Kullanıcı durumunu değiştir (Aktif/Pasif) - ADMIN yetkisi
        [HttpPatch("{id}/durum")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeUserStatus(int id, [FromBody] int durumId)
        {
            var kullanici = await _context.KULLANICILARs.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            kullanici.durum_id = durumId;
            kullanici.guncelleme_tarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı durumu başarıyla güncellendi." });
        }

        // Kullanıcı profil bilgileri (Kendi bilgilerini görme)
        [HttpGet("profile")]
        public async Task<ActionResult<object>> GetProfile()
        {
            var userId = User.FindFirst("nameid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var applicationUser = await _context.ApplicationUsers
                .Include(u => u.Kullanici)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (applicationUser?.Kullanici == null)
            {
                return NotFound(new { message = "Kullanıcı profili bulunamadı." });
            }

            return Ok(new
            {
                applicationUser.Id,
                applicationUser.Ad,
                applicationUser.Soyad,
                applicationUser.Email,
                applicationUser.Telefon,
                applicationUser.KayitTarihi,
                applicationUser.EmailDogrulandi,
                KullanicilarId = applicationUser.KullanicilarId,
                Durum = applicationUser.Kullanici.durum_id
            });
        }

        // Kullanıcı istatistikleri (ADMIN yetkisi)
        [HttpGet("stats")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<object>> GetUserStats()
        {
            var totalUsers = await _context.KULLANICILARs.CountAsync();
            var activeUsers = await _context.KULLANICILARs.CountAsync(k => k.durum_id == 1);
            var verifiedUsers = await _context.KULLANICILARs.CountAsync(k => k.email_dogrulandi);
            var identityUsers = await _context.ApplicationUsers.CountAsync();

            return Ok(new
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                VerifiedUsers = verifiedUsers,
                IdentityUsers = identityUsers,
                IntegratedUsers = await _context.ApplicationUsers.CountAsync(u => u.KullanicilarId.HasValue)
            });
        }
    }
}