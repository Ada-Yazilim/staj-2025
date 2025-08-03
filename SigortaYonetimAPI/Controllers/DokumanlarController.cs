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
    [Authorize]
    public class DokumanlarController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DokumanlarController(SigortaYonetimDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Tüm dökümanları getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DokumanDto>>> GetDokumanlar()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = _context.DOKUMANLARs
                .Include(d => d.musteri)
                .Include(d => d.police)
                .Include(d => d.kategori)
                .AsQueryable();

            if (userRole == "USER")
            {
                var user = await _context.Users.FindAsync(int.Parse(userId ?? "0"));
                if (user?.KullanicilarId != null)
                {
                    query = query.Where(d => d.musteri.kullanici_id == user.KullanicilarId);
                }
            }

            var dokumanListRaw = await query
                .Select(d => new {
                    d.id,
                    d.dosya_adi,
                    d.orijinal_dosya_adi,
                    d.dosya_yolu,
                    d.dosya_boyutu,
                    d.mime_type,
                    d.kategori_id,
                    kategori_adi = d.kategori.deger_aciklama,
                    d.dosya_turu,
                    d.aciklama,
                    d.yuklenme_tarihi,
                    d.musteri_id,
                    musteri = d.musteri,
                    d.police_id,
                    police_no = d.police.police_no
                })
                .ToListAsync();

            var dokumanlar = dokumanListRaw.Select(d => new DokumanDto
            {
                Id = d.id,
                DosyaAdi = d.dosya_adi ?? string.Empty,
                OrijinalDosyaAdi = d.orijinal_dosya_adi,
                DosyaYolu = d.dosya_yolu,
                DosyaBoyutu = d.dosya_boyutu ?? 0,
                MimeType = d.mime_type,
                KategoriId = d.kategori_id,
                KategoriAdi = d.kategori_adi,
                DosyaTuru = d.dosya_turu,
                Aciklama = d.aciklama,
                YuklenmeTarihi = d.yuklenme_tarihi,
                MusteriId = d.musteri_id,
                MusteriAdi = d.musteri != null ? ((d.musteri.ad ?? "") + " " + (d.musteri.soyad ?? "")).Trim() : null,
                PoliceId = d.police_id,
                PoliceNo = d.police_no
            }).ToList();

            return Ok(dokumanlar);
        }

        // Belirli bir dökümanı getir
        [HttpGet("{id}")]
        public async Task<ActionResult<DokumanDetayDto>> GetDokuman(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dokuman = await _context.DOKUMANLARs
                .Include(d => d.musteri)
                .Include(d => d.police)
                .Include(d => d.kategori)
                .FirstOrDefaultAsync(d => d.id == id);

            if (dokuman == null)
                return NotFound("Döküman bulunamadı");

            if (userRole == "USER")
            {
                var user = await _context.Users.FindAsync(int.Parse(userId ?? "0"));
                if (user?.KullanicilarId != dokuman.musteri?.kullanici_id)
                    return Unauthorized("Bu dökümana erişim yetkiniz yok");
            }

            var dokumanDetay = new DokumanDetayDto
            {
                Id = dokuman.id,
                DosyaAdi = dokuman.dosya_adi,
                OrijinalDosyaAdi = dokuman.orijinal_dosya_adi,
                DosyaYolu = dokuman.dosya_yolu,
                DosyaBoyutu = dokuman.dosya_boyutu ?? 0,
                MimeType = dokuman.mime_type,
                KategoriId = dokuman.kategori_id,
                KategoriAdi = dokuman.kategori?.deger_aciklama,
                DosyaTuru = dokuman.dosya_turu,
                Aciklama = dokuman.aciklama,
                YuklenmeTarihi = dokuman.yuklenme_tarihi,
                MusteriId = dokuman.musteri_id,
                MusteriAdi = dokuman.musteri != null ? ((dokuman.musteri.ad ?? "") + " " + (dokuman.musteri.soyad ?? "")).Trim() : null,
                PoliceId = dokuman.police_id,
                PoliceNo = dokuman.police?.police_no
            };

            return Ok(dokumanDetay);
        }

        // Döküman yükle
        [HttpPost("upload")]
        public async Task<ActionResult<DokumanDetayDto>> UploadDokuman([FromForm] UploadDokumanDto uploadDto)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (uploadDto.Dosya == null || uploadDto.Dosya.Length == 0)
                return BadRequest("Dosya seçilmedi");

            // Dosya boyutu kontrolü (10MB)
            if (uploadDto.Dosya.Length > 10 * 1024 * 1024)
                return BadRequest("Dosya boyutu 10MB'dan büyük olamaz");

            // Dosya türü kontrolü
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".xls", ".xlsx" };
            var fileExtension = Path.GetExtension(uploadDto.Dosya.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Geçersiz dosya türü. Sadece PDF, DOC, DOCX, JPG, PNG, XLS, XLSX dosyaları yüklenebilir.");

            // Dosya adı oluştur
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "dokumanlar");
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.Dosya.CopyToAsync(stream);
            }

            // Veritabanına kaydet
            var dokuman = new DOKUMANLAR
            {
                iliski_id = uploadDto.IliskiId,
                musteri_id = uploadDto.MusteriId,
                police_id = uploadDto.PoliceId,
                iliski_tipi_id = uploadDto.IliskiTipiId,
                dosya_adi = fileName,
                orijinal_dosya_adi = uploadDto.Dosya.FileName,
                dosya_yolu = filePath,
                dosya_boyutu = (int)uploadDto.Dosya.Length,
                mime_type = uploadDto.Dosya.ContentType,
                kategori_id = uploadDto.KategoriId,
                dosya_turu = fileExtension,
                aciklama = uploadDto.Aciklama,
                yukleyen_kullanici_id = int.Parse(userId),
                yuklenme_tarihi = DateTime.Now,
                yukleme_tarihi = DateTime.Now
            };

            _context.DOKUMANLARs.Add(dokuman);
            await _context.SaveChangesAsync();

            // Yüklenen dökümanı döndür
            return await GetDokuman(dokuman.id);
        }

        // Döküman indir
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDokuman(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dokuman = await _context.DOKUMANLARs
                .Include(d => d.musteri)
                .FirstOrDefaultAsync(d => d.id == id);

            if (dokuman == null)
                return NotFound("Döküman bulunamadı");

            if (userRole == "USER")
            {
                var user = await _context.Users.FindAsync(int.Parse(userId));
                if (user?.KullanicilarId != dokuman.musteri?.kullanici_id)
                    return Unauthorized("Bu dökümana erişim yetkiniz yok");
            }

            if (!System.IO.File.Exists(dokuman.dosya_yolu))
                return NotFound("Dosya bulunamadı");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(dokuman.dosya_yolu);
            var fileName = dokuman.orijinal_dosya_adi ?? dokuman.dosya_adi;

            return File(fileBytes, dokuman.mime_type ?? "application/octet-stream", fileName);
        }

        // Döküman sil
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDokuman(int id)
        {
            var dokuman = await _context.DOKUMANLARs.FindAsync(id);
            if (dokuman == null)
                return NotFound("Döküman bulunamadı");

            // Dosyayı fiziksel olarak sil
            if (System.IO.File.Exists(dokuman.dosya_yolu))
            {
                System.IO.File.Delete(dokuman.dosya_yolu);
            }

            // Veritabanından sil
            _context.DOKUMANLARs.Remove(dokuman);
            await _context.SaveChangesAsync();

            return Ok("Döküman başarıyla silindi");
        }

        // Döküman kategorilerini getir
        [HttpGet("kategoriler")]
        public async Task<ActionResult<IEnumerable<object>>> GetDokumanKategorileri()
        {
            var kategoriler = await _context.DURUM_TANIMLARIs
                .Where(d => d.tablo_adi == "DOKUMANLAR" && d.alan_adi == "kategori")
                .Select(d => new { d.id, KategoriAdi = d.deger_aciklama })
                .ToListAsync();

            return Ok(kategoriler);
        }
    }
} 