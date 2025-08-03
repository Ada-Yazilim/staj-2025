using SigortaYonetimAPI.Models;

namespace SigortaYonetimAPI.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        string RefreshToken(string token, string refreshToken);
    }
} 