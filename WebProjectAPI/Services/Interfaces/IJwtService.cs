using WebProjectAPI.DTOs;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IJwtService
    {
        JwtResult GenerateJwt(
            int userId,
            string email,
            string fullName,
             string tenantName,
            int? tenantId,
            bool isPlatformUser,
            List<string> roles,
            List<string> permissions);
       
    }
}
