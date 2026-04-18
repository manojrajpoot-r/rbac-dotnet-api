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
using WebProjectAPI.Services;
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
        public IActionResult UserLogin(UserLoginRequest request)
        {
            
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);

            if (user == null)
                return Unauthorized(new { success = false, message = "User not found" });

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { success = false, message = "Invalid password" });

            var token = _jwtService.GenerateJwt(user.Id, user.Email);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Login Successfully!",
                token = token,
                refreshToken = refreshToken.Token
            });
        }

      

        [HttpPost("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            var token = _context.RefreshTokens
                .FirstOrDefault(x => x.Token == refreshToken && !x.IsRevoked);

            if (token == null || token.ExpiryDate < DateTime.Now)
                return Unauthorized();

            // ❗ old token revoke
            token.IsRevoked = true;

            // ❗ new refresh token
            var newRefreshToken = new RefreshToken
            {
                UserId = token.UserId,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.Now.AddDays(7),
                IsRevoked = false
            };


            _context.RefreshTokens.Add(newRefreshToken);

            var user = _context.Users.FirstOrDefault(x => x.Id == token.UserId);
            var newJwt = _jwtService.GenerateJwt(token.UserId, user.Email);
            _context.SaveChanges();

            return Ok(new
            {
                token = newJwt,
                refreshToken = newRefreshToken.Token
            });
        }
    }
}