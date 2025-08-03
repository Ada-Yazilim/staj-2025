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
    public class MusterilerController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public MusterilerController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/Musteriler
        [HttpGet]
        public async Task<IActionResult> GetMusteriler([FromQuery] MusteriSearchDto searchDto, [FromQuery] int? kullanici_id = null)
        {
            try
            {
                var query = _context.MUSTERILERs
                    .Include(m => m.cinsiyet)
                    .Include(m => m.medeni_durum)
                    .Include(m => m.egitim_durumu)
                    .AsQueryable();

                // KULLANICI rolü için sadece kendi verilerini göster
                if (kullanici_id.HasValue)
                {
                    query = query.Where(m => m.id == kullanici_id.Value);
                }

                // Filtreleme
                if (!string.IsNullOrEmpty(searchDto.arama_metni))
                {
                    query = query.Where(m => 
                        m.musteri_no.Contains(searchDto.arama_metni) ||
                        (m.ad ?? "").Contains(searchDto.arama_metni) ||
                        (m.soyad ?? "").Contains(searchDto.arama_metni) ||
                        (m.eposta ?? "").Contains(searchDto.arama_metni) ||
                        (m.telefon ?? "").Contains(searchDto.arama_metni) ||
                        (m.tc_kimlik_no ?? "").Contains(searchDto.arama_metni));
                }

                if (!string.IsNullOrEmpty(searchDto.musteri_no))
                    query = query.Where(m => m.musteri_no.Contains(searchDto.musteri_no));

                if (!string.IsNullOrEmpty(searchDto.tc_kimlik_no))
                    query = query.Where(m => m.tc_kimlik_no == searchDto.tc_kimlik_no);

                if (!string.IsNullOrEmpty(searchDto.eposta))
                    query = query.Where(m => (m.eposta ?? "").Contains(searchDto.eposta));

                if (!string.IsNullOrEmpty(searchDto.telefon))
                    query = query.Where(m => (m.telefon ?? "").Contains(searchDto.telefon) || 
                                              (m.telefon ?? "").Contains(searchDto.telefon));

                if (!string.IsNullOrEmpty(searchDto.adres_il))
                    query = query.Where(m => m.adres_il == searchDto.adres_il);

                if (searchDto.blacklist_mi.HasValue)
                    query = query.Where(m => m.blacklist_mi == searchDto.blacklist_mi.Value);

                if (searchDto.kayit_tarihi_baslangic.HasValue)
                    query = query.Where(m => m.kayit_tarihi >= searchDto.kayit_tarihi_baslangic.Value);

                if (searchDto.kayit_tarihi_bitis.HasValue)
                    query = query.Where(m => m.kayit_tarihi <= searchDto.kayit_tarihi_bitis.Value);

                // Sıralama
                query = searchDto.siralama switch
                {
                    "ad_asc" => query.OrderBy(m => m.ad ?? "").ThenBy(m => m.soyad ?? ""),
                    "ad_desc" => query.OrderByDescending(m => m.ad ?? "").ThenByDescending(m => m.soyad ?? ""),
                    "kayit_tarihi_asc" => query.OrderBy(m => m.kayit_tarihi),
                    "musteri_no_asc" => query.OrderBy(m => m.musteri_no),
                    "musteri_no_desc" => query.OrderByDescending(m => m.musteri_no),
                    _ => query.OrderByDescending(m => m.kayit_tarihi)
                };

                var totalCount = await query.CountAsync();

                var musteriler = await query
                    .Skip((searchDto.sayfa - 1) * searchDto.sayfa_boyutu)
                    .Take(searchDto.sayfa_boyutu)
                    .Select(m => new MusteriListDto
                    {
                        id = m.id,
                        musteri_no = m.musteri_no,
                        ad = m.ad,
                        soyad = m.soyad,
                        eposta = m.eposta,
                        telefon = m.telefon,
                        adres_il = m.adres_il,
                        blacklist_mi = m.blacklist_mi,
                        kayit_tarihi = m.kayit_tarihi
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Data = musteriler,
                    TotalCount = totalCount,
                    Page = searchDto.sayfa,
                    PageSize = searchDto.sayfa_boyutu,
                    TotalPages = (int)Math.Ceiling((double)totalCount / searchDto.sayfa_boyutu)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Müşteriler listelenirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Musteriler/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusteri(int id)
        {
            try
            {
                var musteri = await _context.MUSTERILERs
                    .Include(m => m.cinsiyet)
                    .Include(m => m.medeni_durum)
                    .Include(m => m.egitim_durumu)
                    .FirstOrDefaultAsync(m => m.id == id);

                if (musteri == null)
                {
                    return NotFound("Müşteri bulunamadı");
                }

                // İstatistikler hesapla
                var policeIstatistikleri = await _context.POLISELERs
                    .Where(p => p.musteri_id == id)
                    .GroupBy(p => 1)
                    .Select(g => new
                    {
                        toplam_police_sayisi = g.Count(),
                        toplam_prim_tutari = g.Sum(p => p.brut_prim ?? 0),
                        aktif_police_sayisi = g.Count(p => p.bitis_tarihi > DateTime.Now)
                    })
                    .FirstOrDefaultAsync();

                var hasarSayisi = await _context.HASAR_DOSYALARs
                    .CountAsync(h => h.musteri_id == id);

                var musteriDetay = new MusteriDetayDto
                {
                    id = musteri.id,
                    musteri_no = musteri.musteri_no,
                    ad = musteri.ad,
                    soyad = musteri.soyad,
                    eposta = musteri.eposta,
                    telefon = musteri.telefon,
    
                    dogum_tarihi = musteri.dogum_tarihi,
                    cinsiyet_id = musteri.cinsiyet_id,
                    cinsiyet_adi = musteri.cinsiyet?.deger_aciklama,
                    medeni_durum_id = musteri.medeni_durum_id,
                    medeni_durum_adi = musteri.medeni_durum?.deger_aciklama,
                    meslek = musteri.meslek,
                    egitim_durumu_id = musteri.egitim_durumu_id,
                    egitim_durumu_adi = musteri.egitim_durumu?.deger_aciklama,
                    aylik_gelir = musteri.aylik_gelir,
                    adres_il = musteri.adres_il,
                    adres_ilce = musteri.adres_ilce,
                    adres_mahalle = musteri.adres_mahalle,
                    adres_detay = musteri.adres_detay,
                    posta_kodu = musteri.posta_kodu,
                    not_bilgileri = musteri.not_bilgileri,
                    blacklist_mi = musteri.blacklist_mi,
                    blacklist_nedeni = musteri.blacklist_nedeni,
                    kayit_tarihi = musteri.kayit_tarihi,
                    guncelleme_tarihi = musteri.guncelleme_tarihi,
                    kaydeden_kullanici = musteri.kaydeden_kullanici,
                    toplam_police_sayisi = policeIstatistikleri?.toplam_police_sayisi ?? 0,
                    toplam_prim_tutari = policeIstatistikleri?.toplam_prim_tutari ?? 0,
                    aktif_police_sayisi = policeIstatistikleri?.aktif_police_sayisi ?? 0,
                    hasar_sayisi = hasarSayisi
                };

                return Ok(musteriDetay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Müşteri bilgileri alınırken hata oluştu: {ex.Message}");
            }
        }

        // POST: api/Musteriler
        [HttpPost]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> CreateMusteri([FromBody] MusteriCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Dublicate kontrolleri
                if (!string.IsNullOrEmpty(createDto.tc_kimlik_no))
                {
                    var mevcutTc = await _context.MUSTERILERs
                        .AnyAsync(m => m.tc_kimlik_no == createDto.tc_kimlik_no);
                    if (mevcutTc)
                    {
                        return BadRequest("Bu TC Kimlik numarası ile kayıtlı müşteri bulunmaktadır");
                    }
                }

                if (!string.IsNullOrEmpty(createDto.eposta))
                {
                    var mevcutEposta = await _context.MUSTERILERs
                        .AnyAsync(m => m.eposta == createDto.eposta);
                    if (mevcutEposta)
                    {
                        return BadRequest("Bu e-posta adresi ile kayıtlı müşteri bulunmaktadır");
                    }
                }

                if (!string.IsNullOrEmpty(createDto.telefon))
                {
                    var mevcutTelefon = await _context.MUSTERILERs
                        .AnyAsync(m => m.telefon == createDto.telefon);
                    if (mevcutTelefon)
                    {
                        return BadRequest("Bu telefon numarası ile kayıtlı müşteri bulunmaktadır");
                    }
                }

                // Müşteri numarası oluştur
                var musteriNo = await GenerateMusteriNo();

                var musteri = new MUSTERILER
                {
                    musteri_no = musteriNo,
                    ad = createDto.ad,
                    soyad = createDto.soyad,
                    eposta = createDto.eposta,
                    telefon = createDto.telefon,
                    dogum_tarihi = createDto.dogum_tarihi,
                    cinsiyet_id = createDto.cinsiyet_id,
                    medeni_durum_id = createDto.medeni_durum_id,
                    meslek = createDto.meslek,
                    egitim_durumu_id = createDto.egitim_durumu_id,
                    aylik_gelir = createDto.aylik_gelir,
                    adres_il = createDto.adres_il,
                    adres_ilce = createDto.adres_ilce,
                    adres_mahalle = createDto.adres_mahalle,
                    adres_detay = createDto.adres_detay,
                    posta_kodu = createDto.posta_kodu,
                    not_bilgileri = createDto.not_bilgileri,
                    blacklist_mi = createDto.blacklist_mi ?? false,
                    blacklist_nedeni = createDto.blacklist_nedeni,
                    kayit_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now,
                    kaydeden_kullanici = User.Identity?.Name ?? "System"
                };

                _context.MUSTERILERs.Add(musteri);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMusteri), new { id = musteri.id }, new { id = musteri.id, musteri_no = musteri.musteri_no });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Müşteri oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/Musteriler/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMusteri(int id, [FromBody] MusteriUpdateDto updateDto)
        {
            try
            {
                if (id != updateDto.id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var musteri = await _context.MUSTERILERs.FindAsync(id);
                if (musteri == null)
                {
                    return NotFound("Müşteri bulunamadı");
                }

                // Kullanıcının kendi profilini güncelleyip güncelleyemeyeceğini kontrol et
                var currentUserId = User.FindFirst("kullanicilarId")?.Value;
                var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                
                // ADMIN ve ACENTE tüm müşterileri güncelleyebilir
                if (!userRoles.Contains("ADMIN") && !userRoles.Contains("ACENTE"))
                {
                    // KULLANICI sadece kendi profilini güncelleyebilir
                    if (currentUserId == null || musteri.kullanici_id.ToString() != currentUserId)
                    {
                        return Forbid();
                    }
                }

                // Dublicate kontrolleri (kendisi hariç)
                if (!string.IsNullOrEmpty(updateDto.tc_kimlik_no))
                {
                    var mevcutTc = await _context.MUSTERILERs
                        .AnyAsync(m => m.tc_kimlik_no == updateDto.tc_kimlik_no && m.id != id);
                    if (mevcutTc)
                    {
                        return BadRequest("Bu TC Kimlik numarası ile kayıtlı başka bir müşteri bulunmaktadır");
                    }
                }

                if (!string.IsNullOrEmpty(updateDto.eposta))
                {
                    var mevcutEposta = await _context.MUSTERILERs
                        .AnyAsync(m => m.eposta == updateDto.eposta && m.id != id);
                    if (mevcutEposta)
                    {
                        return BadRequest("Bu e-posta adresi ile kayıtlı başka bir müşteri bulunmaktadır");
                    }
                }

                if (!string.IsNullOrEmpty(updateDto.telefon))
                {
                    var mevcutTelefon = await _context.MUSTERILERs
                        .AnyAsync(m => m.telefon == updateDto.telefon && m.id != id);
                    if (mevcutTelefon)
                    {
                        return BadRequest("Bu telefon numarası ile kayıtlı başka bir müşteri bulunmaktadır");
                    }
                }

                // Güncelleme
                musteri.ad = updateDto.ad;
                musteri.soyad = updateDto.soyad;
                musteri.eposta = updateDto.eposta;
                musteri.telefon = updateDto.telefon;
    
                musteri.dogum_tarihi = updateDto.dogum_tarihi;
                musteri.cinsiyet_id = updateDto.cinsiyet_id;
                musteri.medeni_durum_id = updateDto.medeni_durum_id;
                musteri.meslek = updateDto.meslek;
                musteri.egitim_durumu_id = updateDto.egitim_durumu_id;
                musteri.aylik_gelir = updateDto.aylik_gelir;
                musteri.adres_il = updateDto.adres_il;
                musteri.adres_ilce = updateDto.adres_ilce;
                musteri.adres_mahalle = updateDto.adres_mahalle;
                musteri.adres_detay = updateDto.adres_detay;
                musteri.posta_kodu = updateDto.posta_kodu;
                musteri.not_bilgileri = updateDto.not_bilgileri;
                musteri.blacklist_mi = updateDto.blacklist_mi ?? false;
                musteri.blacklist_nedeni = updateDto.blacklist_nedeni;
                musteri.guncelleme_tarihi = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Müşteri başarıyla güncellendi", id = musteri.id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Müşteri güncelleme hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Müşteri güncellenirken hata oluştu", error = ex.Message });
            }
        }

        // DELETE: api/Musteriler/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteMusteri(int id)
        {
            try
            {
                var musteri = await _context.MUSTERILERs.FindAsync(id);
                if (musteri == null)
                {
                    return NotFound("Müşteri bulunamadı");
                }

                // İlişkili kayıtlar var mı kontrol et
                var hasPolice = await _context.POLISELERs.AnyAsync(p => p.musteri_id == id);
                var hasHasar = await _context.HASAR_DOSYALARs.AnyAsync(h => h.musteri_id == id);

                if (hasPolice || hasHasar)
                {
                    return BadRequest("Bu müşteriye ait poliçe veya hasar kayıtları bulunduğu için silinemez. Müşteriyi blacklist'e ekleyebilirsiniz.");
                }

                _context.MUSTERILERs.Remove(musteri);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Müşteri başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Müşteri silinirken hata oluştu: {ex.Message}");
            }
        }

        // POST: api/Musteriler/5/toggle-blacklist
        [HttpPost("{id}/toggle-blacklist")]
        [Authorize(Roles = "ADMIN,ACENTE")]
        public async Task<IActionResult> ToggleBlacklist(int id, [FromBody] string? neden = null)
        {
            try
            {
                var musteri = await _context.MUSTERILERs.FindAsync(id);
                if (musteri == null)
                {
                    return NotFound("Müşteri bulunamadı");
                }

                musteri.blacklist_mi = !(musteri.blacklist_mi ?? false);
                musteri.blacklist_nedeni = musteri.blacklist_mi == true ? neden : null;
                musteri.guncelleme_tarihi = DateTime.Now;

                await _context.SaveChangesAsync();

                var durum = musteri.blacklist_mi == true ? "blacklist'e eklendi" : "blacklist'ten çıkarıldı";
                return Ok(new { message = $"Müşteri başarıyla {durum}", blacklist_mi = musteri.blacklist_mi });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Blacklist durumu değiştirilirken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Musteriler/istatistikler
        [HttpGet("istatistikler")]
        public async Task<IActionResult> GetMusteriIstatistikleri()
        {
            try
            {
                var toplamMusteri = await _context.MUSTERILERs.CountAsync();

                var blacklistSayisi = await _context.MUSTERILERs
                    .CountAsync(m => m.blacklist_mi == true);

                var buAyEklenen = await _context.MUSTERILERs
                    .CountAsync(m => m.kayit_tarihi.Month == DateTime.Now.Month && 
                                     m.kayit_tarihi.Year == DateTime.Now.Year);

                var ortalamaGelir = await _context.MUSTERILERs
                    .Where(m => m.aylik_gelir.HasValue)
                    .Select(m => m.aylik_gelir.Value)
                    .DefaultIfEmpty(0)
                    .AverageAsync();

                var ilBazindaDagilim = await _context.MUSTERILERs
                    .Where(m => !string.IsNullOrEmpty(m.adres_il))
                    .GroupBy(m => m.adres_il)
                    .Select(g => new IlBazindaMusteriDto
                    {
                        il_adi = g.Key,
                        musteri_sayisi = g.Count()
                    })
                    .OrderByDescending(x => x.musteri_sayisi)
                    .Take(10)
                    .ToListAsync();

                var istatistikler = new MusteriIstatistikDto
                {
                    toplam_musteri_sayisi = toplamMusteri,
                    blacklist_musteri_sayisi = blacklistSayisi,
                    bu_ay_eklenen_sayisi = buAyEklenen,
                    ortalama_aylık_gelir = ortalamaGelir,
                    il_bazinda_dagilim = ilBazindaDagilim
                };

                return Ok(istatistikler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İstatistikler alınırken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Musteriler/lookup-data
        [HttpGet("lookup-data")]
        public async Task<IActionResult> GetLookupData()
        {
            try
            {
                // Sadece iki müşteri tipi sabit olarak dön
                var musteriTipleri = new List<object>
                {
                    new { id = 1, text = "Bireysel Müşteri" },
                    new { id = 2, text = "Kurumsal Müşteri" }
                };

                var cinsiyetler = await _context.DURUM_TANIMLARIs
                    .Where(d => d.tablo_adi == "MUSTERILER" && d.alan_adi == "cinsiyet_id" && d.aktif_mi)
                    .OrderBy(d => d.siralama)
                    .Select(d => new { id = d.deger_kodu == "ERKEK" ? 1 : 2, text = d.deger_aciklama })
                    .ToListAsync();

                var medeniDurumlar = await _context.DURUM_TANIMLARIs
                    .Where(d => d.tablo_adi == "MUSTERILER" && d.alan_adi == "medeni_durum_id" && d.aktif_mi)
                    .OrderBy(d => d.siralama)
                    .ToListAsync();

                var egitimDurumlari = await _context.DURUM_TANIMLARIs
                    .Where(d => d.tablo_adi == "MUSTERILER" && d.alan_adi == "egitim_durumu_id" && d.aktif_mi)
                    .OrderBy(d => d.siralama)
                    .ToListAsync();

                var medeniDurumlarMapped = medeniDurumlar.Select(d => new { 
                    id = d.deger_kodu == "BEKAR" ? 1 : d.deger_kodu == "EVLI" ? 2 : d.deger_kodu == "BOSANMIŞ" ? 3 : 1, 
                    text = d.deger_aciklama 
                }).ToList();

                var egitimDurumlariMapped = egitimDurumlari.Select(d => new { 
                    id = d.deger_kodu == "ILKOKUL" ? 1 : d.deger_kodu == "ORTAOKUL" ? 2 : d.deger_kodu == "LISE" ? 3 : d.deger_kodu == "UNIVERSITE" ? 4 : d.deger_kodu == "YUKSEKLISANS" ? 5 : d.deger_kodu == "DOKTORA" ? 6 : 1, 
                    text = d.deger_aciklama 
                }).ToList();

                return Ok(new
                {
                    musteri_tipleri = musteriTipleri,
                    cinsiyetler = cinsiyetler,
                    medeni_durumlar = medeniDurumlarMapped,
                    egitim_durumlari = egitimDurumlariMapped
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lookup verileri alınırken hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Musteriler/kullanici/{kullaniciId}
        [HttpGet("kullanici/{kullaniciId}")]
        public async Task<IActionResult> GetMusteriByKullaniciId(int kullaniciId)
        {
            try
            {
                Console.WriteLine($"Kullanıcı ID: {kullaniciId} için müşteri aranıyor...");
                
                var musteri = await _context.MUSTERILERs
                    .FirstOrDefaultAsync(m => m.kullanici_id == kullaniciId);

                if (musteri == null)
                {
                    Console.WriteLine($"Müşteri bulunamadı: kullanici_id = {kullaniciId}");
                    return NotFound(new { message = "Müşteri bulunamadı" });
                }

                Console.WriteLine($"Müşteri bulundu: {musteri.ad} {musteri.soyad}");
                return Ok(musteri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return StatusCode(500, new { message = "Müşteri bilgileri getirilemedi", error = ex.Message });
            }
        }

        private async Task<string> GenerateMusteriNo()
        {
            var yil = DateTime.Now.Year.ToString();
            var prefix = $"MST{yil}";
            
            var sonNo = await _context.MUSTERILERs
                .Where(m => m.musteri_no.StartsWith(prefix))
                .Select(m => m.musteri_no)
                .OrderByDescending(m => m)
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

            return $"{prefix}{siradakiNo:D6}"; // MST2024000001 formatı
        }
    }
} 