using WebProjectAPI.DTOs;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IJwtService
    {
        JwtResult GenerateJwt(int userId, string email);
    }
}
