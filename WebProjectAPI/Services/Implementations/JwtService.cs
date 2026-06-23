using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebProjectAPI.DTOs;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Services.Implementations
{
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
       string fullName,
       string tenantName,
       int? tenantId,
       bool isPlatformUser,
       List<string> roles,
       List<string> permissions) {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("IsPlatformUser", isPlatformUser.ToString()),
                new Claim("FullName", fullName ?? ""),
                new Claim("TenantName", tenantName ?? "")
            };

            // =========================
            // TENANT CLAIM
            // =========================
            if (tenantId.HasValue)
            {
                claims.Add(new Claim(CustomClaims.TenantId, tenantId.Value.ToString()));
            }

            // =========================
            // ROLES
            // =========================
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // =========================
            // PERMISSIONS (IMPORTANT FIX)
            // =========================
            foreach (var permission in permissions.Distinct())
            {
                claims.Add(new Claim(CustomClaims.Permission, permission));
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