using SigortaYonetimAPI.Models;

namespace SigortaYonetimAPI.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<string> GenerateRefreshTokenAsync();
        Task<bool> ValidateTokenAsync(string token);
        Task<string> RefreshTokenAsync(string token, string refreshToken);
    }
} 