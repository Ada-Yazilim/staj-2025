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
    public class OdemelerController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public OdemelerController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // Tüm ödemeleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OdemeDto>>> GetOdemeler()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = _context.ODEMELERs
                .Include(o => o.police)
                .Include(o => o.musteri)
                .Include(o => o.durum)
                .Include(o => o.taksit)
                .AsQueryable();

            if (userRole == "USER")
            {
                var user = await _context.Users.FindAsync(int.Parse(userId ?? "0"));
                            if (user?.KullanicilarId != null)
            {
                query = query.Where(o => o.musteri.kullanici_id == user.KullanicilarId);
            }
            }

            var odemeListRaw = await query
                .Select(o => new {
                    o.id,
                    o.odeme_no,
                    o.police_id,
                    police_no = o.police.police_no,
                    o.musteri_id,
                    musteri = o.musteri,
                    o.odeme_turu,
                    o.tutar,
                    o.durum_id,
                    durum_adi = o.durum.deger_aciklama,
                    o.odeme_tarihi,
                    o.vade_tarihi,
                    o.aciklama
                })
                .ToListAsync();

            var odemeler = odemeListRaw.Select(o => new OdemeDto
            {
                Id = o.id,
                OdemeNo = o.odeme_no,
                PoliceId = o.police_id,
                PoliceNo = o.police_no,
                MusteriId = o.musteri_id,
                MusteriAdi = o.musteri != null ? ((o.musteri.ad ?? "") + " " + (o.musteri.soyad ?? "")).Trim() : null,
                OdemeTuru = o.odeme_turu,
                Tutar = o.tutar,
                DurumId = o.durum_id,
                DurumAdi = o.durum_adi,
                OdemeTarihi = o.odeme_tarihi,
                VadeTarihi = o.vade_tarihi,
                Aciklama = o.aciklama
            }).ToList();

            return Ok(odemeler);
        }

        // Belirli bir ödemeyi getir
        [HttpGet("{id}")]
        public async Task<ActionResult<OdemeDetayDto>> GetOdeme(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var odeme = await _context.ODEMELERs
                .Include(o => o.police)
                .Include(o => o.musteri)
                .Include(o => o.durum)
                .Include(o => o.taksit)
                .FirstOrDefaultAsync(o => o.id == id);

            if (odeme == null)
                return NotFound("Ödeme bulunamadı");

            if (userRole == "USER")
            {
                var user = await _context.Users.FindAsync(int.Parse(userId ?? "0"));
                if (user?.KullanicilarId != odeme.musteri?.kullanici_id)
                    return Unauthorized("Bu ödemeye erişim yetkiniz yok");
            }

            var odemeDetay = new OdemeDetayDto
            {
                Id = odeme.id,
                OdemeNo = odeme.odeme_no,
                PoliceId = odeme.police_id,
                PoliceNo = odeme.police?.police_no ?? "",
                MusteriId = odeme.musteri_id,
                MusteriAdi = odeme.musteri?.ad != null && odeme.musteri?.soyad != null ? ($"{odeme.musteri.ad} {odeme.musteri.soyad}").Trim() : null,
                OdemeTuru = odeme.odeme_turu,
                Tutar = odeme.tutar,
                DurumId = odeme.durum_id,
                DurumAdi = odeme.durum.deger_aciklama,
                OdemeTarihi = odeme.odeme_tarihi,
                VadeTarihi = odeme.vade_tarihi,
                Aciklama = odeme.aciklama,
                TaksitSayisi = odeme.taksit_sayisi ?? 1,
                TaksitTutari = odeme.taksit_tutari ?? odeme.tutar,
                Taksitler = odeme.taksit != null ? new List<TaksitDto>
                {
                    new TaksitDto
                    {
                        Id = odeme.taksit.id,
                        TaksitNo = odeme.taksit.taksit_no,
                        Tutar = odeme.taksit.toplam_tutar,
                        VadeTarihi = odeme.taksit.vade_tarihi,
                        OdemeTarihi = odeme.taksit.odeme_tarihi,
                        DurumAdi = odeme.taksit.durum.deger_aciklama
                    }
                } : new List<TaksitDto>()
            };

            return Ok(odemeDetay);
        }

        // Yeni ödeme oluştur
        [HttpPost]
        public async Task<ActionResult<OdemeDetayDto>> CreateOdeme(CreateOdemeDto createDto)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (createDto.VadeTarihi <= DateTime.Now)
                return BadRequest("Vade tarihi gelecekte olmalıdır.");
            if (createDto.Tutar <= 0)
                return BadRequest("Tutar 0'dan büyük olmalıdır.");
            if (createDto.TaksitSayisi < 1 || createDto.TaksitSayisi > 60)
                return BadRequest("Taksit sayısı 1-60 arasında olmalıdır.");

            // Poliçe kontrolü
            var police = await _context.POLISELERs
                .Include(p => p.musteri)
                .Include(p => p.police_turu)
                .Include(p => p.sigorta_sirketi)
                .FirstOrDefaultAsync(p => p.id == createDto.PoliceId);

            if (police == null)
                return BadRequest("Poliçe bulunamadı");

            // Aktif durum kontrolü
            var aktifDurum = await _context.DURUM_TANIMLARIs
                .FirstOrDefaultAsync(d => d.tablo_adi == "POLISELER" && d.deger_kodu == "AKTIF");
            
            if (police.durum_id != aktifDurum?.id)
                return BadRequest("Sadece aktif poliçeler için ödeme oluşturulabilir");

            // Beklemede durum ID'si
            var bekleyenDurum = await _context.DURUM_TANIMLARIs
                .FirstOrDefaultAsync(d => d.tablo_adi == "ODEMELER" && d.deger_kodu == "BEKLEMEDE");

            // Ödeme numarası oluştur
            var odemeNo = await GenerateOdemeNo();

            var odeme = new ODEMELER
            {
                odeme_no = odemeNo,
                police_id = createDto.PoliceId,
                musteri_id = police.musteri_id,
                odeme_turu = createDto.OdemeTuru,
                tutar = createDto.Tutar,
                durum_id = bekleyenDurum?.id ?? 1,
                odeme_tarihi = DateTime.Now,
                vade_tarihi = createDto.VadeTarihi,
                aciklama = createDto.Aciklama,
                taksit_sayisi = createDto.TaksitSayisi,
                taksit_tutari = createDto.TaksitSayisi > 1 ? createDto.Tutar / createDto.TaksitSayisi : createDto.Tutar,
                olusturma_tarihi = DateTime.Now
            };

            _context.ODEMELERs.Add(odeme);
            await _context.SaveChangesAsync();

            // Taksitli ödeme ise taksitleri oluştur
            if (createDto.TaksitSayisi > 1)
            {
                var taksitTutari = createDto.Tutar / createDto.TaksitSayisi;
                var vadeTarihi = createDto.VadeTarihi;

                for (int i = 1; i <= createDto.TaksitSayisi; i++)
                {
                    var taksit = new TAKSITLER
                    {
                        odeme_id = odeme.id,
                        police_id = createDto.PoliceId,
                        taksit_no = i,
                        ana_para = taksitTutari,
                        toplam_tutar = taksitTutari,
                        vade_tarihi = vadeTarihi,
                        durum_id = bekleyenDurum?.id ?? 1,
                        olusturma_tarihi = DateTime.Now
                    };

                    _context.TAKSITLERs.Add(taksit);
                    vadeTarihi = vadeTarihi.AddMonths(1);
                }

                await _context.SaveChangesAsync();
            }

            // Müşteriye bildirim gönder
            await SendNotificationToCustomer(police.musteri_id, 
                $"Poliçe ödeme planınız oluşturuldu! Ödeme No: {odeme.odeme_no}, Tutar: ₺{odeme.tutar:N2}");

            // Oluşturulan ödemeyi döndür
            return await GetOdeme(odeme.id);
        }

        // Ödeme durumunu güncelle
        [HttpPut("{id}/durum")]
        public async Task<ActionResult> UpdateOdemeDurum(int id, UpdateOdemeDurumDto dto)
        {
            var odeme = await _context.ODEMELERs.FindAsync(id);
            if (odeme == null)
                return NotFound("Ödeme bulunamadı");

            odeme.durum_id = dto.DurumId;
            await _context.SaveChangesAsync();

            return Ok("Ödeme durumu güncellendi");
        }

        // Taksit ödemesi yap
        [HttpPost("taksit-odeme")]
        public async Task<ActionResult> TaksitOdeme(TaksitOdemeDto dto)
        {
            var taksit = await _context.TAKSITLERs
                .Include(t => t.odeme)
                .Include(t => t.police)
                .ThenInclude(p => p.musteri)
                .FirstOrDefaultAsync(t => t.id == dto.TaksitId);

            if (taksit == null)
                return NotFound("Taksit bulunamadı");

            // Beklemede durum kontrolü
            var bekleyenDurum = await _context.DURUM_TANIMLARIs
                .FirstOrDefaultAsync(d => d.tablo_adi == "TAKSITLER" && d.deger_kodu == "BEKLEMEDE");
            
            var odendiDurum = await _context.DURUM_TANIMLARIs
                .FirstOrDefaultAsync(d => d.tablo_adi == "TAKSITLER" && d.deger_kodu == "ODENDI");

            if (taksit.durum_id != bekleyenDurum?.id)
                return BadRequest("Bu taksit zaten ödenmiş veya farklı bir durumda");

            // Ödeme yöntemi kontrolü
            if (string.IsNullOrEmpty(dto.KartNo) || string.IsNullOrEmpty(dto.SonKullanmaTarihi) || 
                string.IsNullOrEmpty(dto.Cvv) || string.IsNullOrEmpty(dto.KartSahibi))
            {
                return BadRequest("Kart bilgileri eksik");
            }

            // Kart numarası format kontrolü
            if (dto.KartNo.Length != 16 || !dto.KartNo.All(char.IsDigit))
                return BadRequest("Geçersiz kart numarası");

            // CVV kontrolü
            if (dto.Cvv.Length != 3 || !dto.Cvv.All(char.IsDigit))
                return BadRequest("Geçersiz CVV");

            // Son kullanma tarihi kontrolü
            if (!DateTime.TryParse(dto.SonKullanmaTarihi, out var sonKullanma) || sonKullanma < DateTime.Now)
                return BadRequest("Kartın son kullanma tarihi geçmiş");

            // Taksit ödemesini gerçekleştir
            taksit.odeme_tarihi = DateTime.Now;
            taksit.durum_id = odendiDurum?.id ?? 2;

            // Tüm taksitler ödendiyse ana ödemeyi de güncelle
            var odeme = taksit.odeme;
            var bekleyenDurumId = bekleyenDurum?.id ?? 0;
            var bekleyenTaksitler = await _context.TAKSITLERs
                .Where(t => t.odeme_id == odeme.id && t.durum_id == bekleyenDurumId)
                .CountAsync();

            if (bekleyenTaksitler == 0)
            {
                var odendiDurumOdeme = await _context.DURUM_TANIMLARIs
                    .FirstOrDefaultAsync(d => d.tablo_adi == "ODEMELER" && d.deger_kodu == "ODENDI");
                odeme.durum_id = odendiDurumOdeme?.id ?? 2;
            }

            await _context.SaveChangesAsync();

            // Müşteriye bildirim gönder
            if (taksit.police?.musteri_id != null)
            {
                await SendNotificationToCustomer(taksit.police.musteri_id, 
                    $"Taksit ödemeniz başarıyla gerçekleştirildi! Taksit No: {taksit.taksit_no}, Tutar: ₺{taksit.toplam_tutar:N2}");
            }

            return Ok(new { 
                message = "Taksit ödemesi başarıyla gerçekleştirildi",
                taksit_no = taksit.taksit_no,
                odeme_tarihi = taksit.odeme_tarihi,
                tutar = taksit.toplam_tutar,
                kalan_taksit = bekleyenTaksitler
            });
        }

        // Ödeme türlerini getir
        [HttpGet("odeme-turleri")]
        public async Task<ActionResult<IEnumerable<object>>> GetOdemeTurleri()
        {
            var odemeTurleri = new[] { "Peşin", "Taksitli", "Kredi Kartı", "Banka Transferi" };
            var durumlar = await _context.DURUM_TANIMLARIs
                .Where(d => d.tablo_adi == "ODEMELER")
                .Select(d => new { d.id, DurumAdi = d.deger_aciklama })
                .ToListAsync();

            return Ok(new { OdemeTurleri = odemeTurleri, Durumlar = durumlar });
        }

        // Ödeme numarası oluştur
        private async Task<string> GenerateOdemeNo()
        {
            var year = DateTime.Now.Year;
            var lastOdeme = _context.ODEMELERs
                .Where(o => o.odeme_no.StartsWith($"ODM{year}"))
                .OrderByDescending(o => o.odeme_no)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastOdeme != null)
            {
                var lastNumber = int.Parse(lastOdeme.odeme_no.Substring(7));
                nextNumber = lastNumber + 1;
            }

            return $"ODM{year}{nextNumber:D6}";
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
                        baslik = "Ödeme Güncellemesi",
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