namespace WebProjectAPI.Services.Implementations
{
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using WebProjectAPI.DTOs;
    using WebProjectAPI.Services.Interfaces;



    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public JwtResult GenerateJwt(
      int userId,
      string email,
      int? tenantId,
      List<string> roles,
      List<string> permissions)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            if (tenantId.HasValue)
            {
                claims.Add(new Claim("TenantId", tenantId.Value.ToString()));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

           
            var expiry = DateTime.UtcNow.AddHours(12);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = expiry
            };
        }
    }
}
