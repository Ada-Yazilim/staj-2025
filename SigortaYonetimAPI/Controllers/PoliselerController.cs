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
    public class PoliselerController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public PoliselerController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/Poliseler
        [HttpGet]
        public async Task<IActionResult> GetPoliseler([FromQuery] int? musteriId = null, [FromQuery] string? durum = null, [FromQuery] int? kullanici_id = null)
        {
            try
            {
                var query = _context.POLISELERs
                    .Include(p => p.musteri)
                    .Include(p => p.police_turu)
                    .Include(p => p.sigorta_sirketi)
                    .Include(p => p.tanzim_eden_kullanici)
                    .Include(p => p.durum)
                    .Include(p => p.teklif)
                    .AsQueryable();

                if (musteriId.HasValue)
                    query = query.Where(p => p.musteri_id == musteriId.Value);

                if (!string.IsNullOrEmpty(durum))
                    query = query.Where(p => p.durum.deger_kodu == durum);

                // KULLANICI rolü için sadece kendi poliçelerini göster
                if (kullanici_id.HasValue)
                {
                    query = query.Where(p => p.musteri_id == kullanici_id.Value);
                }

                var poliseler = await query
                    .OrderByDescending(p => p.tanzim_tarihi)
                    .Select(p => new PoliceListDto
                    {
                        id = p.id,
                        police_no = p.police_no,
                        musteri_adi = ($"{p.musteri.ad} {p.musteri.soyad}").Trim(),
                        police_turu_adi = p.police_turu.urun_adi,
                        sigorta_sirketi_adi = p.sigorta_sirketi.sirket_adi,
                        baslangic_tarihi = p.baslangic_tarihi,
                        bitis_tarihi = p.bitis_tarihi,
                        brut_prim = p.brut_prim,
                        net_prim = p.net_prim,
                        toplam_tutar = p.toplam_tutar,
                        durum_adi = p.durum.deger_aciklama,
                        tanzim_tarihi = p.tanzim_tarihi,
                        tanzim_eden_kullanici = $"{p.tanzim_eden_kullanici.ad} {p.tanzim_eden_kullanici.soyad}",
                        teklif_no = p.teklif != null ? p.teklif.teklif_no : null
                    })
                    .ToListAsync();

                return Ok(poliseler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçeler listelenirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Poliseler/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolice(int id)
        {
            try
            {
                var police = await _context.POLISELERs
                    .Include(p => p.musteri)
                    .Include(p => p.police_turu)
                    .Include(p => p.sigorta_sirketi)
                    .Include(p => p.tanzim_eden_kullanici)
                    .Include(p => p.durum)
                    .Include(p => p.teklif)
                    .FirstOrDefaultAsync(p => p.id == id);

                if (police == null)
                {
                    return NotFound("Poliçe bulunamadı");
                }

                var policeDetay = new PoliceDetayDto
                {
                    id = police.id,
                    police_no = police.police_no,
                    teklif_id = police.teklif_id,
                    teklif_no = police.teklif?.teklif_no,
                    musteri_id = police.musteri_id,
                    musteri_adi = ($"{police.musteri.ad} {police.musteri.soyad}").Trim(),
                    police_turu_id = police.police_turu_id,
                    police_turu_adi = police.police_turu.urun_adi,
                    sigorta_sirketi_id = police.sigorta_sirketi_id,
                    sigorta_sirketi_adi = police.sigorta_sirketi.sirket_adi,
                    tanzim_eden_kullanici_id = police.tanzim_eden_kullanici_id,
                    tanzim_eden_kullanici = $"{police.tanzim_eden_kullanici.ad} {police.tanzim_eden_kullanici.soyad}",
                    risk_bilgileri = police.risk_bilgileri,
                    teminat_bilgileri = police.teminat_bilgileri,
                    baslangic_tarihi = police.baslangic_tarihi,
                    bitis_tarihi = police.bitis_tarihi,
                    brut_prim = police.brut_prim,
                    net_prim = police.net_prim,
                    komisyon_tutari = police.komisyon_tutari,
                    vergi_tutari = police.vergi_tutari,
                    toplam_tutar = police.toplam_tutar,
                    taksit_sayisi = police.taksit_sayisi,
                    durum_id = police.durum_id,
                    durum_adi = police.durum.deger_aciklama,
                    iptal_nedeni = police.iptal_nedeni,
                    iptal_tarihi = police.iptal_tarihi,
                    yenileme_hatirlatma_tarihi = police.yenileme_hatirlatma_tarihi,
                    ozel_sartlar = police.ozel_sartlar,
                    notlar = police.notlar,
                    tanzim_tarihi = police.tanzim_tarihi,
                    guncelleme_tarihi = police.guncelleme_tarihi
                };

                return Ok(policeDetay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe detayı alınırken hata oluştu: {ex.Message}");
            }
        }

        // POST: api/Poliseler
        [HttpPost]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> CreatePolice([FromBody] PoliceCreateDto createDto)
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

                // Teklif kontrolü (varsa)
                POLICE_TEKLIFLERI? teklif = null;
                if (createDto.teklif_id.HasValue)
                {
                    teklif = await _context.POLICE_TEKLIFLERIs
                        .Include(t => t.musteri)
                        .Include(t => t.police_turu)
                        .Include(t => t.sigorta_sirketi)
                        .FirstOrDefaultAsync(t => t.id == createDto.teklif_id.Value);
                    
                    if (teklif == null)
                    {
                        return BadRequest("Poliçe teklifi bulunamadı");
                    }

                    // Teklifin onaylanmış olup olmadığını kontrol et
                    var onaylanmisDurum = await _context.DURUM_TANIMLARIs
                        .FirstOrDefaultAsync(d => d.tablo_adi == "POLICE_TEKLIFLERI" && d.deger_kodu == "ONAYLANDI");
                    
                    if (teklif.durum_id != onaylanmisDurum?.id)
                    {
                        return BadRequest("Sadece onaylanmış tekliflerden poliçe oluşturulabilir");
                    }
                }

                // Aktif durum ID'si
                var aktifDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "POLISELER" && d.deger_kodu == "AKTIF")?.id ?? 1;

                // Poliçe numarası oluştur
                var policeNo = await GeneratePoliceNo();

                // Kullanıcı ID'sini al
                var kullaniciId = int.Parse(User.FindFirst("KullanicilarId")?.Value ?? "1");

                var police = new POLISELER
                {
                    police_no = policeNo,
                    teklif_id = createDto.teklif_id,
                    musteri_id = createDto.musteri_id,
                    police_turu_id = createDto.police_turu_id,
                    sigorta_sirketi_id = createDto.sigorta_sirketi_id,
                    tanzim_eden_kullanici_id = kullaniciId,
                    risk_bilgileri = createDto.risk_bilgileri ?? teklif?.risk_bilgileri,
                    teminat_bilgileri = createDto.teminat_bilgileri ?? teklif?.teminat_bilgileri,
                    baslangic_tarihi = createDto.baslangic_tarihi,
                    bitis_tarihi = createDto.bitis_tarihi,
                    brut_prim = createDto.brut_prim ?? teklif?.brut_prim,
                    net_prim = createDto.net_prim ?? teklif?.net_prim,
                    komisyon_tutari = createDto.komisyon_tutari ?? teklif?.komisyon_tutari,
                    vergi_tutari = createDto.vergi_tutari ?? teklif?.vergi_tutari,
                    toplam_tutar = createDto.toplam_tutar ?? teklif?.toplam_tutar,
                    taksit_sayisi = createDto.taksit_sayisi,
                    durum_id = aktifDurumId,
                    ozel_sartlar = createDto.ozel_sartlar,
                    notlar = createDto.notlar,
                    tanzim_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now
                };

                _context.POLISELERs.Add(police);
                await _context.SaveChangesAsync();

                // Müşteriye bildirim gönder
                await SendNotificationToCustomer(musteri.id, 
                    $"Poliçeniz oluşturuldu! Poliçe No: {police.police_no}");

                // Komisyon hesaplaması yap
                if (police.komisyon_tutari.HasValue && police.komisyon_tutari > 0)
                {
                    await CreateCommissionCalculation(police, kullaniciId);
                }

                return CreatedAtAction(nameof(GetPolice), new { id = police.id }, 
                    new { 
                        id = police.id, 
                        police_no = police.police_no,
                        musteri_adi = ($"{musteri.ad} {musteri.soyad}").Trim(),
                        police_turu = policeTuru.urun_adi,
                        sigorta_sirketi = sigortaSirketi.sirket_adi,
                        toplam_tutar = police.toplam_tutar
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/Poliseler/5/iptal
        [HttpPut("{id}/iptal")]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> IptalPolice(int id, [FromBody] string iptalNedeni)
        {
            try
            {
                var police = await _context.POLISELERs.FindAsync(id);
                if (police == null)
                {
                    return NotFound("Poliçe bulunamadı");
                }

                // İptal edildi durum ID'si
                var iptalDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "POLISELER" && d.deger_kodu == "IPTAL")?.id ?? 2;

                police.durum_id = iptalDurumId;
                police.iptal_nedeni = iptalNedeni;
                police.iptal_tarihi = DateTime.Now;
                police.guncelleme_tarihi = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Poliçe iptal edildi", police_no = police.police_no });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Poliçe iptal edilirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Poliseler/lookup-data
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
                    .Where(d => d.tablo_adi == "POLISELER" && d.aktif_mi)
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

        private async Task<string> GeneratePoliceNo()
        {
            var yil = DateTime.Now.Year.ToString();
            var prefix = $"POL{yil}";
            
            var sonNo = await _context.POLISELERs
                .Where(p => p.police_no.StartsWith(prefix))
                .Select(p => p.police_no)
                .OrderByDescending(p => p)
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

            return $"{prefix}{siradakiNo:D6}"; // POL2024000001 formatı
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
                        baslik = "Poliçe Oluşturuldu",
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

        private async Task CreateCommissionCalculation(POLISELER police, int acenteKullaniciId)
        {
            try
            {
                var hesaplanmisDurum = await _context.DURUM_TANIMLARIs
                    .FirstOrDefaultAsync(d => d.tablo_adi == "KOMISYON_HESAPLARI" && d.deger_kodu == "HESAPLANDI");

                var komisyon = new KOMISYON_HESAPLARI
                {
                    police_id = police.id,
                    acente_kullanici_id = acenteKullaniciId,
                    brut_prim = police.brut_prim ?? 0,
                    komisyon_orani = 15.00m, // Varsayılan %15 komisyon
                    komisyon_tutari = police.komisyon_tutari ?? 0,
                    net_komisyon = (police.komisyon_tutari ?? 0) * 0.85m, // %15 stopaj
                    stopaj_tutari = (police.komisyon_tutari ?? 0) * 0.15m,
                    durum_id = hesaplanmisDurum?.id ?? 1,
                    hesaplama_tarihi = DateTime.Now,
                    aciklama = $"Poliçe No: {police.police_no} için komisyon hesaplaması"
                };

                _context.KOMISYON_HESAPLARIs.Add(komisyon);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Komisyon hesaplanamasa bile ana işlem devam etsin
                Console.WriteLine($"Komisyon hesaplanamadı: {ex.Message}");
            }
        }
    }
} 