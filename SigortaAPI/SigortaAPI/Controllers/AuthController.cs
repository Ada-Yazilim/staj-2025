using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaAPI.Data;
using SigortaAPI.Dtos;
using SigortaAPI.Models;

namespace SigortaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Email kullanımda mı kontrol et
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Bu e-posta zaten kayıtlı.");

            // Şifreyi hash'le (örnek olarak basit, ileride BCrypt kullanabiliriz)
            var passwordHash = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(dto.Password)
            );

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { user.Id, user.Username, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized("E-posta veya şifre yanlış.");

            var incomingHash = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(dto.Password)
            );
            if (user.PasswordHash != incomingHash)
                return Unauthorized("E-posta veya şifre yanlış.");

            // Şu an dummy token döndürüyoruz; ileride JWT ekleriz
            var token = "dummy-token";

            return Ok(new { token });
        }
    }
}
