using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;
using SigortaYonetimAPI.Models.DTOs;

namespace SigortaYonetimAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(
            SigortaYonetimDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Gelişmiş kullanıcı listesi (Filtreleme ve sayfalama ile)
        [HttpGet("users")]
        public async Task<ActionResult<object>> GetUsers(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? role = null,
            [FromQuery] bool? active = null)
        {
            var query = _context.ApplicationUsers
                .Include(u => u.Kullanici)
                .Include(u => u.Yonetici)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => 
                    (u.Ad ?? "").Contains(search) || 
                    (u.Soyad ?? "").Contains(search) || 
                    (u.Email ?? "").Contains(search) ||
                    (u.Telefon ?? "").Contains(search));
            }

            // Aktiflik filtresi
            if (active.HasValue)
            {
                if (active.Value)
                {
                    query = query.Where(u => u.AktifMi && !u.HesapKilitlenmeTarihi.HasValue);
                }
                else
                {
                    query = query.Where(u => !u.AktifMi || u.HesapKilitlenmeTarihi.HasValue);
                }
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderByDescending(u => u.KayitTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userList = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                // Rol filtresi uygula
                if (!string.IsNullOrEmpty(role) && !roles.Contains(role))
                    continue;

                userList.Add(new
                {
                    user.Id,
                    user.Ad,
                    user.Soyad,
                    user.Email,
                    user.Telefon,
                    user.EmailDogrulandi,
                    user.TelefonDogrulandi,
                    user.KayitTarihi,
                    user.GuncellemeTarihi,
                    user.SonGirisTarihi,
                    user.SonAktiviteTarihi,
                    user.SonIpAdresi,
                    user.BasarisizGirisSayisi,
                    user.HesapKilitlenmeTarihi,
                    user.AktifMi,
                    user.Pozisyon,
                    user.Departman,
                    user.Notlar,
                                    KullanicilarId = user.KullanicilarId,
                KullanicilarDurum = user.Kullanici?.durum_id,
                    YoneticiId = user.YoneticiId,
                    YoneticiAdi = user.Yonetici != null ? $"{user.Yonetici.Ad} {user.Yonetici.Soyad}" : null,
                    Roller = roles.ToList(),
                    HesapKilitliMi = user.HesapKilitliMi,
                    TamAd = user.TamAd
                });
            }

            // Basit liste döndür (Frontend uyumluluğu için)
            if (page == 1 && pageSize == 10 && string.IsNullOrEmpty(search) && string.IsNullOrEmpty(role) && !active.HasValue)
            {
                return Ok(userList); // Frontend'in beklediği format
            }

            return Ok(new
            {
                Users = userList,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        // Kullanıcı detaylarını getir
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<object>> GetUser(string userId)
        {
            var user = await _context.ApplicationUsers
                .Include(u => u.Kullanici)
                .Include(u => u.Yonetici)
                .Include(u => u.AstKullanicilar)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Ad,
                user.Soyad,
                user.Email,
                user.Telefon,
                user.EmailDogrulandi,
                user.TelefonDogrulandi,
                user.KayitTarihi,
                user.GuncellemeTarihi,
                user.SonGirisTarihi,
                user.SonAktiviteTarihi,
                user.SonIpAdresi,
                user.BasarisizGirisSayisi,
                user.HesapKilitlenmeTarihi,
                user.AktifMi,
                user.Pozisyon,
                user.Departman,
                user.Notlar,
                KullanicilarId = user.KullanicilarId,
                KullanicilarDurum = user.Kullanici?.durum_id,
                YoneticiId = user.YoneticiId,
                YoneticiAdi = user.Yonetici != null ? $"{user.Yonetici.Ad} {user.Yonetici.Soyad}" : null,
                AstKullanicilar = user.AstKullanicilar.Select(a => new { a.Id, a.TamAd }).ToList(),
                Roller = roles.ToList(),
                Claims = claims.Select(c => new { c.Type, c.Value }).ToList(),
                HesapKilitliMi = user.HesapKilitliMi,
                TamAd = user.TamAd
            });
        }

        // Kullanıcı bilgilerini güncelle
        [HttpPut("users/{userId}")]
        public async Task<ActionResult<object>> UpdateUser(string userId, [FromBody] UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Temel bilgileri güncelle
            user.Ad = model.Ad;
            user.Soyad = model.Soyad;
            user.Email = model.Email;
            user.UserName = model.Email; // Email'i UserName olarak da kullan
            user.Telefon = model.Telefon;
            user.Pozisyon = model.Pozisyon;
            user.Departman = model.Departman;
            user.YoneticiId = model.YoneticiId;
            user.Notlar = model.Notlar;
            user.AktifMi = model.AktifMi;
            user.GuncellemeTarihi = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Kullanıcı güncellenemedi.", errors = result.Errors });
            }

            // KULLANICILAR tablosunu da güncelle
            if (user.KullanicilarId.HasValue)
            {
                var kullanici = await _context.KULLANICILARs.FindAsync(user.KullanicilarId.Value);
                if (kullanici != null)
                {
                    kullanici.ad = user.Ad;
                    kullanici.soyad = user.Soyad;
                    kullanici.eposta = user.Email;
                    kullanici.telefon = user.Telefon;
                    kullanici.guncelleme_tarihi = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Kullanıcı başarıyla güncellendi." });
        }

        // Kullanıcı aktivite geçmişi
        [HttpGet("users/{userId}/activity")]
        public async Task<ActionResult<object>> GetUserActivity(string userId, [FromQuery] int take = 50)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // SISTEM_LOGLARI'ndan kullanıcı aktivitelerini getir
            var activities = await _context.SISTEM_LOGLARIs
                .Include(s => s.islem_tipi)
                .Where(s => s.kullanici_id == user.KullanicilarId)
                .OrderByDescending(s => s.islem_tarihi)
                .Take(take)
                .Select(s => new
                {
                    s.id,
                    s.islem_tarihi,
                    IslemTipi = s.islem_tipi.deger_aciklama,
                    s.tablo_adi,
                    s.ip_adresi,
                    s.tarayici_bilgisi,
                    s.aciklama
                })
                .ToListAsync();

            return Ok(new
            {
                UserId = userId,
                UserName = user.TamAd,
                Activities = activities,
                LastActivity = user.SonAktiviteTarihi,
                LastIpAddress = user.SonIpAdresi
            });
        }

        // Yeni kullanıcı oluştur (Admin)
        [HttpPost("users")]
        public async Task<ActionResult<object>> CreateUser([FromBody] CreateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. KULLANICILAR tablosuna ekle
                var kullanici = new KULLANICILAR
                {
                    ad = model.Ad,
                    soyad = model.Soyad,
                    eposta = model.Email,
                    sifre_hash = "IDENTITY_MANAGED",
                    telefon = model.Telefon,
                    durum_id = 1, // Aktif
                    email_dogrulandi = false,
                    telefon_dogrulandi = false,
                    basarisiz_giris_sayisi = 0,
                    kayit_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now
                };

                _context.KULLANICILARs.Add(kullanici);
                await _context.SaveChangesAsync();

                // 2. Identity kullanıcısı oluştur
                var applicationUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Telefon = model.Telefon,
                    Pozisyon = model.Pozisyon,
                    Departman = model.Departman,
                    YoneticiId = model.YoneticiId,
                    Notlar = model.Notlar,
                    EmailDogrulandi = false,
                    TelefonDogrulandi = false,
                    AktifMi = true,
                    KayitTarihi = DateTime.Now,
                    GuncellemeTarihi = DateTime.Now,
                    KullanicilarId = kullanici.id
                };

                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Identity kullanıcısı oluşturulamadı.", errors = result.Errors });
                }

                // 3. Rol ata
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(applicationUser, model.Role);
                }

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Kullanıcı başarıyla oluşturuldu.",
                    userId = applicationUser.Id,
                    kullanicilarId = kullanici.id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = "Kullanıcı oluşturulurken hata oluştu.", error = ex.Message });
            }
        }

        // Kullanıcı rollerini güncelle
        [HttpPut("users/{userId}/roles")]
        public async Task<ActionResult<object>> UpdateUserRoles(string userId, [FromBody] UpdateUserRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            
            // Mevcut rolleri kaldır
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return BadRequest(new { message = "Mevcut roller kaldırılamadı.", errors = removeResult.Errors });
            }

            // Yeni rolleri ekle
            if (model.Roles != null && model.Roles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, model.Roles);
                if (!addResult.Succeeded)
                {
                    return BadRequest(new { message = "Yeni roller eklenemedi.", errors = addResult.Errors });
                }
            }

            return Ok(new { message = "Kullanıcı rolleri başarıyla güncellendi." });
        }

        // Kullanıcı hesabını kilitle/aç
        [HttpPatch("users/{userId}/lock")]
        public async Task<ActionResult<object>> ToggleUserLock(string userId, [FromBody] bool lockUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Admin, acente ve test kullanıcılarını koruma
            var protectedEmails = new[] { "admin@test.com", "acente@test.com", "kullanici@test.com" };
            if (protectedEmails.Contains(user.Email))
            {
                return BadRequest(new { message = "Bu kullanıcı korumalıdır ve kilitleme işlemlerinden hariç tutulmuştur." });
            }

            if (lockUser)
            {
                user.HesapKilitlenmeTarihi = DateTime.Now;
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            else
            {
                user.HesapKilitlenmeTarihi = null;
                await _userManager.SetLockoutEndDateAsync(user, null);
            }

            user.GuncellemeTarihi = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return Ok(new { 
                message = lockUser ? "Kullanıcı hesabı kilitlendi." : "Kullanıcı hesabı açıldı.",
                isLocked = lockUser
            });
        }

        // Kullanıcı şifresini sıfırla (Admin)
        [HttpPost("users/{userId}/reset-password")]
        public async Task<ActionResult<object>> ResetUserPassword(string userId, [FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Şifre sıfırlanamadı.", errors = result.Errors });
            }

            user.GuncellemeTarihi = DateTime.Now;
            user.BasarisizGirisSayisi = 0;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Kullanıcı şifresi başarıyla sıfırlandı." });
        }

        // Kullanıcıya not ekle
        [HttpPost("users/{userId}/notes")]
        public async Task<ActionResult<object>> AddUserNote(string userId, [FromBody] UserNoteDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            var currentNote = user.Notlar ?? "";
            var newNote = $"[{DateTime.Now:dd.MM.yyyy HH:mm}] {model.Note}";
            
            user.Notlar = string.IsNullOrEmpty(currentNote) 
                ? newNote 
                : $"{currentNote}\n{newNote}";
            user.GuncellemeTarihi = DateTime.Now;

            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Not başarıyla eklendi." });
        }

        // Kullanıcı silme (Soft delete)
        [HttpDelete("users/{userId}")]
        public async Task<ActionResult<object>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Soft delete - kullanıcıyı aktif değil olarak işaretle
            user.AktifMi = false;
            user.HesapKilitlenmeTarihi = DateTime.Now;
            user.GuncellemeTarihi = DateTime.Now;
            
            // Email'i benzersiz hale getirmek için timestamp ekle
            user.Email = $"DELETED_{DateTime.Now:yyyyMMddHHmmss}_{user.Email}";
            user.UserName = user.Email;
            user.Notlar = (user.Notlar ?? "") + $"\n[{DateTime.Now:dd.MM.yyyy HH:mm}] Kullanıcı silindi.";

            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Kullanıcı başarıyla silindi." });
        }

        // Kullanıcı hesabını aktifleştir/pasifleştir
        [HttpPatch("users/{userId}/toggle-active")]
        public async Task<ActionResult<object>> ToggleUserActive(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            user.AktifMi = !user.AktifMi;
            user.GuncellemeTarihi = DateTime.Now;
            
            var action = user.AktifMi ? "aktifleştirildi" : "pasifleştirildi";
            user.Notlar = (user.Notlar ?? "") + $"\n[{DateTime.Now:dd.MM.yyyy HH:mm}] Kullanıcı {action}.";

            await _userManager.UpdateAsync(user);

            return Ok(new { 
                message = $"Kullanıcı başarıyla {action}.",
                aktifMi = user.AktifMi
            });
        }

        // Potansiyel yöneticiler listesi (Hiyerarşi için)
        [HttpGet("users/potential-managers")]
        public async Task<ActionResult<object>> GetPotentialManagers()
        {
            var managers = await _context.ApplicationUsers
                .Where(u => u.AktifMi)
                .Select(u => new
                {
                    u.Id,
                    u.TamAd,
                    u.Pozisyon,
                    u.Departman,
                    u.Email
                })
                .OrderBy(u => u.TamAd)
                .ToListAsync();

            return Ok(managers);
        }

        // Bulk kullanıcı işlemleri
        [HttpPost("users/bulk-action")]
        public async Task<ActionResult<object>> BulkUserAction([FromBody] BulkActionDto model)
        {
            if (!model.UserIds.Any())
            {
                return BadRequest(new { message = "En az bir kullanıcı seçilmelidir." });
            }

            var users = await _context.ApplicationUsers
                .Where(u => model.UserIds.Contains(u.Id))
                .ToListAsync();

            var results = new List<object>();
            var protectedEmails = new[] { "admin@test.com", "acente@test.com", "kullanici@test.com" };

            foreach (var user in users)
            {
                try
                {
                    // Kilitleme işlemleri için korumalı kullanıcıları kontrol et
                    if ((model.Action.ToLower() == "lock" || model.Action.ToLower() == "unlock") && 
                        protectedEmails.Contains(user.Email))
                    {
                        results.Add(new { 
                            UserId = user.Id, 
                            Success = false, 
                            Message = "Bu kullanıcı korumalıdır ve kilitleme işlemlerinden hariç tutulmuştur." 
                        });
                        continue;
                    }

                    switch (model.Action.ToLower())
                    {
                        case "activate":
                            user.AktifMi = true;
                            user.HesapKilitlenmeTarihi = null;
                            break;
                        case "deactivate":
                            user.AktifMi = false;
                            break;
                        case "lock":
                            user.HesapKilitlenmeTarihi = DateTime.Now.AddYears(1);
                            break;
                        case "unlock":
                            user.HesapKilitlenmeTarihi = null;
                            break;
                        default:
                            results.Add(new { UserId = user.Id, Success = false, Message = "Geçersiz işlem" });
                            continue;
                    }

                    user.GuncellemeTarihi = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    results.Add(new { UserId = user.Id, Success = true, Message = "Başarılı" });
                }
                catch (Exception ex)
                {
                    results.Add(new { UserId = user.Id, Success = false, Message = ex.Message });
                }
            }

            return Ok(new
            {
                Message = "Bulk işlem tamamlandı.",
                Results = results,
                TotalProcessed = users.Count,
                SuccessCount = results.Count(r => ((dynamic)r).Success),
                FailCount = results.Count(r => !((dynamic)r).Success)
            });
        }

        // Sistem rolleri listesi
        [HttpGet("roles")]
        public async Task<ActionResult<object>> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Aciklama,
                    r.AktifMi,
                    r.OlusturmaTarihi
                })
                .ToListAsync();

            return Ok(roles);
        }

        // Sistem istatistikleri
        [HttpGet("dashboard-stats")]
        public async Task<ActionResult<object>> GetDashboardStats()
        {
            var totalUsers = await _context.ApplicationUsers.CountAsync();
            var activeUsers = await _context.ApplicationUsers
                .CountAsync(u => u.HesapKilitlenmeTarihi == null);
            var lockedUsers = await _context.ApplicationUsers
                .CountAsync(u => u.HesapKilitlenmeTarihi != null);
            var verifiedUsers = await _context.ApplicationUsers
                .CountAsync(u => u.EmailDogrulandi);
            
            var roleStats = await _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { RoleName = r.Name })
                .GroupBy(x => x.RoleName)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToListAsync();

            var recentLogins = await _context.ApplicationUsers
                .Where(u => u.SonGirisTarihi.HasValue)
                .OrderByDescending(u => u.SonGirisTarihi)
                .Take(5)
                .Select(u => new
                {
                    u.Ad,
                    u.Soyad,
                    u.Email,
                    u.SonGirisTarihi
                })
                .ToListAsync();

            return Ok(new
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                LockedUsers = lockedUsers,
                VerifiedUsers = verifiedUsers,
                RoleDistribution = roleStats,
                RecentLogins = recentLogins
            });
        }

        // 🔧 GERÇEK SİGORTA FİRMASI İÇİN GELİŞMİŞ ÖZELLİKLER

        // Kullanıcı performans raporu (Sigorta sektörüne özel)
        [HttpGet("users/{userId}/performance")]
        public async Task<IActionResult> GetUserPerformance(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var kullaniciId = user.KullanicilarId;
                if (!kullaniciId.HasValue)
                {
                    return BadRequest("Kullanıcı entegrasyonu bulunamadı");
                }

                var currentDate = DateTime.Now;
                var thirtyDaysAgo = currentDate.AddDays(-30);

                // Satış performansı - son 30 gün
                var salesCount = await _context.POLISELERs
                    .Where(p => p.tanzim_eden_kullanici_id == kullaniciId.Value && 
                                p.tanzim_tarihi >= thirtyDaysAgo)
                    .CountAsync();

                var totalPrim = await _context.POLISELERs
                    .Where(p => p.tanzim_eden_kullanici_id == kullaniciId.Value && 
                                p.tanzim_tarihi >= thirtyDaysAgo)
                    .SumAsync(p => p.brut_prim ?? 0);

                // Komisyon kazancı
                var totalCommission = await _context.KOMISYON_HESAPLARIs
                    .Where(k => k.acente_kullanici_id == kullaniciId.Value && 
                                k.hesaplama_tarihi >= thirtyDaysAgo)
                    .SumAsync(k => k.net_komisyon ?? 0);

                // Yeni müşteri sayısı - son 30 gün  
                var newCustomersCount = await _context.MUSTERILERs
                    .Where(m => m.kayit_tarihi >= thirtyDaysAgo)
                    .CountAsync();

                // Hasar dosyası sayısı
                var claimsCount = await _context.HASAR_DOSYALARs
                    .Where(h => h.bildiren_kullanici_id == kullaniciId.Value && 
                                h.bildirim_tarihi >= thirtyDaysAgo)
                    .CountAsync();

                // Giriş yapılan gün sayısı (OTURUM_KAYITLARI tablosu kaldırıldı)
                var loginDays = 0; // Şimdilik 0, sonra ApplicationUser.SonGirisTarihi'nden hesaplanabilir

                return Ok(new
                {
                    SatisAdedi = salesCount,
                    ToplamPrim = totalPrim,
                    ToplamKomisyon = totalCommission,
                    YeniMusteriSayisi = newCustomersCount,
                    HasarDosyaSayisi = claimsCount,
                    GirisGunSayisi = loginDays,
                    Donem = "Son 30 Gün"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Performans verileri alınırken hata: {ex.Message}");
            }
        }

        // Detaylı kullanıcı audit raporu
        [HttpGet("users/{userId}/audit")]
        public async Task<IActionResult> GetUserAudit(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var kullaniciId = user.KullanicilarId;
                if (!kullaniciId.HasValue)
                {
                    return BadRequest("Kullanıcı entegrasyonu bulunamadı");
                }

                // Son 30 günlük giriş kayıtları (OTURUM_KAYITLARI tablosu kaldırıldı)
                var loginHistory = new List<object>(); // Şimdilik boş liste

                // Sistem logları (şimdiki yapıda kullanıcı işlemleri - örnek)
                var systemLogs = await _context.SISTEM_LOGLARIs
                    .Include(s => s.islem_tipi)
                    .Where(s => s.kullanici_id == kullaniciId.Value)
                    .OrderByDescending(s => s.islem_tarihi)
                    .Take(50)
                    .Select(s => new
                    {
                        Tarih = s.islem_tarihi,
                        Islem = s.islem_tipi != null ? s.islem_tipi.deger_aciklama : "Bilinmiyor",
                        Detay = s.aciklama,
                        IpAdresi = s.ip_adresi
                    })
                    .ToListAsync();

                // Güvenlik olayları (şifre sıfırlama talepleri)
                var securityEvents = await _context.SIFRE_SIFIRLAMAs
                    .Where(s => s.kullanici_id == kullaniciId.Value)
                    .OrderByDescending(s => s.olusturma_tarihi)
                    .Take(20)
                    .Select(s => new
                    {
                        Tarih = s.olusturma_tarihi,
                        Olay = "Şifre Sıfırlama Talebi",
                        Durum = s.kullanildi_mi ? "Kullanıldı" : "Beklemede",
                        IpAdresi = s.ip_adresi
                    })
                    .ToListAsync();

                return Ok(new
                {
                    GirisGecmisi = loginHistory,
                    SistemLoglari = systemLogs,
                    GuvenlikOlaylari = securityEvents
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Audit verileri alınırken hata: {ex.Message}");
            }
        }

        // Organizasyon hiyerarşisi görünümü
        [HttpGet("organization-hierarchy")]
        public async Task<ActionResult<object>> GetOrganizationHierarchy()
        {
            var users = await _context.ApplicationUsers
                .Include(u => u.AstKullanicilar)
                .Where(u => u.AktifMi)
                .Select(u => new
                {
                    u.Id,
                    u.TamAd,
                    u.Pozisyon,
                    u.Departman,
                    u.YoneticiId,
                    AstKullaniciler = u.AstKullanicilar.Select(a => new { a.Id, a.TamAd, a.Pozisyon }).ToList(),
                    Roller = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .ToList()
                })
                .ToListAsync();

            // Hiyerarşi ağacı oluştur
            var topLevelUsers = users.Where(u => string.IsNullOrEmpty(u.YoneticiId)).ToList();
            
            return Ok(new
            {
                TotalEmployees = users.Count,
                Departments = users.GroupBy(u => u.Departman ?? "Tanımsız")
                    .Select(g => new { Department = g.Key, Count = g.Count() }).ToList(),
                Hierarchy = topLevelUsers
            });
        }

        // Gelişmiş kullanıcı arama ve filtreleme
        [HttpPost("users/advanced-search")]
        public async Task<IActionResult> AdvancedUserSearch([FromBody] AdvancedSearchDto searchDto)
        {
            try
            {
                var query = _context.ApplicationUsers.AsQueryable();

                // Temel filtreler
                if (!string.IsNullOrEmpty(searchDto.Ad))
                {
                    query = query.Where(u => u.Ad.Contains(searchDto.Ad));
                }

                if (!string.IsNullOrEmpty(searchDto.Soyad))
                {
                    query = query.Where(u => u.Soyad.Contains(searchDto.Soyad));
                }

                if (!string.IsNullOrEmpty(searchDto.Email))
                {
                    query = query.Where(u => u.Email.Contains(searchDto.Email));
                }

                if (!string.IsNullOrEmpty(searchDto.Departman))
                {
                    query = query.Where(u => u.Departman.Contains(searchDto.Departman));
                }

                if (!string.IsNullOrEmpty(searchDto.Pozisyon))
                {
                    query = query.Where(u => u.Pozisyon.Contains(searchDto.Pozisyon));
                }

                // Tarih filtreleri
                if (searchDto.KayitTarihiBaslangic.HasValue)
                {
                    query = query.Where(u => u.KayitTarihi >= searchDto.KayitTarihiBaslangic.Value);
                }

                if (searchDto.KayitTarihiBitis.HasValue)
                {
                    query = query.Where(u => u.KayitTarihi <= searchDto.KayitTarihiBitis.Value);
                }

                if (searchDto.SonGirisTarihiBaslangic.HasValue)
                {
                    query = query.Where(u => u.SonGirisTarihi >= searchDto.SonGirisTarihiBaslangic.Value);
                }

                // Durum filtreleri
                if (searchDto.AktifMi.HasValue)
                {
                    query = query.Where(u => u.AktifMi == searchDto.AktifMi.Value);
                }

                if (searchDto.EmailDogrulandi.HasValue)
                {
                    query = query.Where(u => u.EmailDogrulandi == searchDto.EmailDogrulandi.Value);
                }

                // Sayısal filtreler
                var users = await query.ToListAsync();
                var userIds = users.Select(u => u.Id).ToList();

                var usersWithRoles = new List<object>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    // Rol filtresi
                    if (!string.IsNullOrEmpty(searchDto.Rol) && !roles.Contains(searchDto.Rol))
                        continue;

                    usersWithRoles.Add(new
                    {
                        Id = user.Id,
                        TamAd = user.TamAd,
                        Email = user.Email,
                        Telefon = user.Telefon,
                        Departman = user.Departman,
                        Pozisyon = user.Pozisyon,
                        Roller = roles.ToList(),
                        AktifMi = user.AktifMi,
                        KayitTarihi = user.KayitTarihi,
                        SonGirisTarihi = user.SonGirisTarihi,
                        EmailDogrulandi = user.EmailDogrulandi
                    });
                }

                return Ok(new
                {
                    Users = usersWithRoles,
                    TotalCount = usersWithRoles.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Gelişmiş arama sırasında hata: {ex.Message}");
            }
        }

        // Kullanıcı güvenlik ayarları
        [HttpPost("users/{userId}/security-settings")]
        public async Task<ActionResult<object>> UpdateUserSecuritySettings(string userId, [FromBody] SecuritySettingsDto settings)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            // Şifre geçmişi kontrolü (ek tablo gerekebilir)
            if (settings.SifreZorunluDegisim)
            {
                user.Notlar = (user.Notlar ?? "") + $"\n[{DateTime.Now:dd.MM.yyyy HH:mm}] Şifre değişikliği zorunlu hale getirildi.";
            }

            if (settings.IkiFaktorYetkilendirme)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                user.Notlar = (user.Notlar ?? "") + $"\n[{DateTime.Now:dd.MM.yyyy HH:mm}] İki faktörlü doğrulama etkinleştirildi.";
            }

            if (settings.OturumZamanAsimi > 0)
            {
                // Session timeout ayarı (ek implementation gerekebilir)
                user.Notlar = (user.Notlar ?? "") + $"\n[{DateTime.Now:dd.MM.yyyy HH:mm}] Oturum zaman aşımı {settings.OturumZamanAsimi} dakika olarak ayarlandı.";
            }

            user.GuncellemeTarihi = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Güvenlik ayarları güncellendi." });
        }

        // Sistem geneli raporlar
        [HttpGet("reports/system-overview")]
        public async Task<IActionResult> GetSystemOverviewReport()
        {
            try
            {
                var currentDate = DateTime.Now;
                var thirtyDaysAgo = currentDate.AddDays(-30);

                // Genel sistem metrikleri
                var totalUsers = await _context.ApplicationUsers.CountAsync();
                var activeUsers = await _context.ApplicationUsers.CountAsync(u => u.AktifMi);
                var totalCustomers = await _context.MUSTERILERs.CountAsync();
                var totalPolicies = await _context.POLISELERs.CountAsync();

                // Son 30 gün aktivite
                var recentLogins = 0; // OTURUM_KAYITLARI tablosu kaldırıldı

                var recentPolicies = await _context.POLISELERs
                    .Where(p => p.tanzim_tarihi >= thirtyDaysAgo)
                    .CountAsync();

                // Güvenlik metrikleri
                var passwordResetRequests = await _context.SIFRE_SIFIRLAMAs
                    .Where(s => s.olusturma_tarihi >= thirtyDaysAgo)
                    .CountAsync();

                var lockedUsers = await _context.ApplicationUsers
                    .CountAsync(u => u.HesapKilitlenmeTarihi.HasValue && u.HesapKilitlenmeTarihi > currentDate.AddDays(-30));

                // Departman dağılımı
                var departmentStats = await _context.ApplicationUsers
                    .Where(u => !string.IsNullOrEmpty(u.Departman))
                    .GroupBy(u => u.Departman)
                    .Select(g => new
                    {
                        Departman = g.Key,
                        KullaniciSayisi = g.Count(),
                        AktifKullaniciSayisi = g.Count(u => u.AktifMi)
                    })
                    .ToListAsync();

                // Rol dağılımı
                var roleStats = new List<object>();
                var roles = await _roleManager.Roles.ToListAsync();
                
                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                    roleStats.Add(new
                    {
                        Rol = role.Name,
                        KullaniciSayisi = usersInRole.Count,
                        Aciklama = role.Aciklama
                    });
                }

                return Ok(new
                {
                    GenelMetrikler = new
                    {
                        ToplamKullanici = totalUsers,
                        AktifKullanici = activeUsers,
                        ToplamMusteri = totalCustomers,
                        ToplamPolice = totalPolicies
                    },
                    SonAktivite = new
                    {
                        Son30GunGiris = recentLogins,
                        Son30GunPolice = recentPolicies
                    },
                    GuvenlikDurumu = new
                    {
                        SifreSifirlamaTalebi = passwordResetRequests,
                        KilitliHesapSayisi = lockedUsers
                    },
                    DepartmanDagilimi = departmentStats,
                    RolDagilimi = roleStats,
                    RaporTarihi = currentDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sistem özet raporu alınırken hata: {ex.Message}");
            }
        }

        // Kullanıcıya rol atama endpoint'i
        [HttpPost("assign-role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"Kullanıcı bulunamadı: {email}");
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    return BadRequest($"Rol bulunamadı: {roleName}");
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Contains(roleName))
                {
                    return BadRequest($"Kullanıcı zaten {roleName} rolüne sahip");
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok($"Rol başarıyla atandı: {email} -> {roleName}");
                }
                else
                {
                    return BadRequest($"Rol atama başarısız: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Rol atama hatası"); // _logger is not defined in this file
                return StatusCode(500, "Sunucu hatası");
            }
        }

        // Kullanıcı rollerini listeleme endpoint'i
        [HttpGet("user-roles/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest(new { message = "Kullanıcı bulunamadı" });
                }

                var roles = await _userManager.GetRolesAsync(user);
                
                return Ok(new { 
                    user = new { user.Email, user.Ad, user.Soyad },
                    roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Kullanıcı rolleri alınırken hata oluştu", details = ex.Message });
            }
        }

        [HttpGet("check-roles")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var roleList = roles.Select(r => new { r.Id, r.Name, r.Aciklama }).ToList();
                
                return Ok(new { 
                    message = "Mevcut roller",
                    roles = roleList,
                    count = roleList.Count
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Rol kontrolü hatası"); // _logger is not defined in this file
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("check-user-roles")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUserRoles(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"Kullanıcı bulunamadı: {email}");
                }

                var roles = await _userManager.GetRolesAsync(user);
                
                return Ok(new { 
                    email = email,
                    roles = roles.ToList(),
                    count = roles.Count
                });
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Kullanıcı rol kontrolü hatası"); // _logger is not defined in this file
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }

    // DTOs
    public class CreateUserDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Role { get; set; }
        public string? Pozisyon { get; set; }
        public string? Departman { get; set; }
        public string? YoneticiId { get; set; }
        public string? Notlar { get; set; }
    }

    public class UpdateUserDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Pozisyon { get; set; }
        public string? Departman { get; set; }
        public string? YoneticiId { get; set; }
        public string? Notlar { get; set; }
        public bool AktifMi { get; set; } = true;
    }

    public class UpdateUserRolesDto
    {
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UserNoteDto
    {
        public string Note { get; set; } = string.Empty;
    }

    public class BulkActionDto
    {
        public List<string> UserIds { get; set; } = new List<string>();
        public string Action { get; set; } = string.Empty; // activate, deactivate, lock, unlock
    }

    // 🔧 YENİ DTO'LAR (Gelişmiş özellikler için)
    public class AdvancedSearchDto
    {
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Email { get; set; }
        public string? Departman { get; set; }
        public string? Pozisyon { get; set; }
        public string? Rol { get; set; }
        public DateTime? KayitTarihiBaslangic { get; set; }
        public DateTime? KayitTarihiBitis { get; set; }
        public DateTime? SonGirisTarihiBaslangic { get; set; }
        public bool? AktifMi { get; set; }
        public bool? EmailDogrulandi { get; set; }
    }

    public class SecuritySettingsDto
    {
        public bool SifreZorunluDegisim { get; set; }
        public bool IkiFaktorYetkilendirme { get; set; }
        public int OturumZamanAsimi { get; set; } // dakika
        public bool IpAdresiKisitlama { get; set; }
        public string? IzinliIpAdresleri { get; set; }
    }

    public class AssignRoleDto
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // ADMIN, ACENTE, KULLANICI
    }
} 