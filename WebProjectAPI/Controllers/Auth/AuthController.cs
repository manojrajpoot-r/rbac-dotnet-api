using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs;
using WebProjectAPI.Models;
using WebProjectAPI.Services.Interfaces;
namespace WebProjectAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IConfiguration configuration, IJwtService jwtService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
            _jwtService = jwtService;
        }

        // 🔐 REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = ModelState
                });
            }

            var exists = _context.Users.Any(x => x.Email == request.Email);
            if (exists)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email already exists"
                });
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,

            };

            user.Password = _passwordHasher.HashPassword(user, request.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "User Registered Successfully",
                data = new
                {
                    user.Id,
                    user.Name,
                    user.Email,

                }
            });
        }

        // 🔐 LOGIN
        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and Password are required"
                });
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid password"
                });
            }

            //  JWT generate
            var jwt = _jwtService.GenerateJwt(user.Id, user.Email);

            //  Role fetch
            var roles = _context.UserRoles
                     .Where(ur => ur.UserId == user.Id)
                     .Select(ur => ur.Role.Name)
                     .Distinct()
                     .ToList();

            //  Permission fetch
            var permissions = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(_context.Permissions,
                    rp => rp,
                    p => p.Id,
                    (rp, p) => p.Name)
                .Distinct()
                .ToList();

            //  Refresh token
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);

            _context.SaveChanges();

            var expiresIn = (int)(jwt.Expiry - DateTime.UtcNow).TotalSeconds;

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login Successfully!",
                Data = new
                {
                    accessToken = jwt.Token,
                    refreshToken = refreshToken.Token,
                    expiresIn = expiresIn,

                    user = new
                    {
                        id = user.Id,
                        name=user.Name,
                        email = user.Email,
                        roles = roles,
                        permissions = permissions
                    }
                }
            });
        }


        [HttpPost("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            var token = _context.RefreshTokens
                .FirstOrDefault(x => x.Token == refreshToken && !x.IsRevoked);

            if (token == null || token.ExpiryDate < DateTime.UtcNow)
                return Unauthorized(new { success = false, message = "Invalid refresh token" });

            // ❗ old token revoke
            token.IsRevoked = true;

            // ❗ new refresh token
            var newRefreshToken = new RefreshToken
            {
                UserId = token.UserId,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshToken);

            var user = _context.Users.FirstOrDefault(x => x.Id == token.UserId);

            // ✅ new JWT
            var jwt = _jwtService.GenerateJwt(user.Id, user.Email);

            var expiresIn = (int)(jwt.Expiry - DateTime.UtcNow).TotalSeconds;

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Token refreshed",
                data = new
                {
                    accessToken = jwt.Token,
                    refreshToken = newRefreshToken.Token,
                    expiresIn = expiresIn,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email
                    }
                }
            });
        }

        [HttpPost("logout-all")]
        public IActionResult LogoutAll(int userId)
        {
            var tokens = _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToList();
            if (tokens == null)
                return NotFound(new { success = false, message = "Token not found" });

            foreach (var t in tokens)
            {
                t.IsRevoked = true;
            }

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Logged out from all devices"
            });
        }
    }
}