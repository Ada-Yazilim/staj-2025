using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;
using SigortaYonetimAPI.Models.DTOs;
using SigortaYonetimAPI.Services;

namespace SigortaYonetimAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly SigortaYonetimDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ITokenService tokenService,
            ILogger<AuthController> logger,
            SigortaYonetimDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Geçersiz veri gönderildi.",
                    });
                }

                // E-posta kontrolü
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Bu e-posta adresi zaten kayıtlı.",
                    });
                }

                // Önce KULLANICILAR tablosuna ekle
                var aktifDurumId = _context.DURUM_TANIMLARIs
                    .FirstOrDefault(d => d.tablo_adi == "KULLANICILAR" && d.deger_kodu == "AKTIF")?.id ?? 1;

                var kullanici = new KULLANICILAR
                {
                    ad = request.Ad,
                    soyad = request.Soyad,
                    eposta = request.Email,
                    telefon = request.Telefon,
                    durum_id = aktifDurumId,
                    email_dogrulandi = true,
                    telefon_dogrulandi = false,
                    sifre_hash = "IDENTITY_MANAGED", // Identity tarafından yönetiliyor
                    kayit_tarihi = DateTime.Now,
                    guncelleme_tarihi = DateTime.Now
                };

                _context.KULLANICILARs.Add(kullanici);
                await _context.SaveChangesAsync();

                // Şimdi Identity kullanıcısı oluştur ve KULLANICILAR'a bağla
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    Ad = request.Ad,
                    Soyad = request.Soyad,
                    Telefon = request.Telefon,
                    EmailConfirmed = true,
                    KullanicilarId = kullanici.id, // 🔗 BAĞLANTI KURULDU!
                    KayitTarihi = DateTime.Now,
                    GuncellemeTarihi = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    // Identity kullanıcısı oluşturulamazsa KULLANICILAR kaydını da sil
                    _context.KULLANICILARs.Remove(kullanici);
                    await _context.SaveChangesAsync();
                    
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    });
                }

                // Varsayılan rol ata (KULLANICI)
                await _userManager.AddToRoleAsync(user, "KULLANICI");

                // Token oluştur
                var token = await _tokenService.GenerateJwtTokenAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Kayıt başarılı.",
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Ad = user.Ad,
                        Soyad = user.Soyad,
                        Email = user.Email!,
                        Telefon = user.Telefon,
                        Roles = roles.ToList(),
                        // KULLANICILAR tablosundan ek bilgiler
                        KullanicilarId = kullanici.id,
                        KayitTarihi = kullanici.kayit_tarihi,
                        EmailDogrulandi = kullanici.email_dogrulandi
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register işleminde hata oluştu");
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Sunucu hatası oluştu.",
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Geçersiz veri gönderildi.",
                    });
                }

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(new AuthResponseDto
                    {
                        Success = false,
                        Message = "E-posta veya şifre hatalı.",
                    });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(new AuthResponseDto
                    {
                        Success = false,
                        Message = "E-posta veya şifre hatalı.",
                    });
                }

                // Son giriş tarihini güncelle
                user.SonGirisTarihi = DateTime.Now;
                user.BasarisizGirisSayisi = 0;
                await _userManager.UpdateAsync(user);

                // Token oluştur
                var token = await _tokenService.GenerateJwtTokenAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                // KULLANICILAR tablosundan ek bilgileri al
                KULLANICILAR? kullaniciDetay = null;
                if (user.KullanicilarId.HasValue)
                {
                    kullaniciDetay = await _context.KULLANICILARs
                        .FirstOrDefaultAsync(k => k.id == user.KullanicilarId.Value);
                }

                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Giriş başarılı.",
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Ad = user.Ad,
                        Soyad = user.Soyad,
                        Email = user.Email!,
                        Telefon = user.Telefon,
                        Roles = roles.ToList(),
                        // KULLANICILAR tablosundan ek bilgiler
                        KullanicilarId = user.KullanicilarId,
                        KayitTarihi = kullaniciDetay?.kayit_tarihi,
                        EmailDogrulandi = kullaniciDetay?.email_dogrulandi ?? false
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işleminde hata oluştu");
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Sunucu hatası oluştu.",
                });
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult<AuthResponseDto>> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Çıkış başarılı.",
            });
        }
    }
} 