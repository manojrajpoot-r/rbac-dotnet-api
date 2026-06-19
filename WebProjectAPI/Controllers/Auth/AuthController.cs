
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _context.Users
                .AnyAsync(x =>
                    x.Email == request.Email &&
                    x.TenantId == request.TenantId);

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
                FullName = request.Name,
                Email = request.Email,
                TenantId = request.TenantId
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(user, request.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var customerRole = await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.Name == "Customer" &&
                    x.TenantId == request.TenantId);

            if (customerRole != null)
            {
                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = customerRole.Id
                });

                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                success = true,
                message = "User Registered Successfully",
                data = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.TenantId
                }
            });
        }
        // 🔐 LOGIN


        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            if (request.IsPlatformUser)
            {
                var admin = _context.PlatformUsers
                    .FirstOrDefault(x => x.Email == request.Email);

                if (admin == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Super admin not found"
                    });
                }

                var verify = _passwordHasher.VerifyHashedPassword(
                    null,
                    admin.PasswordHash,
                    request.Password);

                if (verify == PasswordVerificationResult.Failed)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid password"
                    });
                }

                var roles = new List<string>
                {
                    "SuperAdmin"
                };

                var permissions = _context.Permissions
                    .Select(x => x.Name)
                    .ToList();

                var jwt = _jwtService.GenerateJwt(
                  admin.Id,
                  admin.Email,
                  null,
                  true,
                  roles,
                  permissions);

                return Ok(new
                {
                    success = true,
                    message = "Login Successfully",
                    data = new
                    {
                        accessToken = jwt.Token,
                        refreshToken = "",
                        expiresAt = jwt.Expiry,
                        user = new
                        {
                            admin.Id,
                            admin.Email,
                            admin.FullName,
                            roles = roles,
                            permissions = permissions
                        }
                    }
                });
            }

            var user = _context.Users
                .FirstOrDefault(x => x.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid password"
                });
            }

            var rolesList = _context.UserRoles
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Role.Name)
                .Distinct()
                .ToList();

            var permissionsList = _context.UserRoles
                   .Where(ur =>
                       ur.UserId == user.Id &&
                       ur.TenantId == user.TenantId)
                   .Join(
                       _context.RolePermissions.Where(rp =>
                           rp.TenantId == user.TenantId),
                       ur => ur.RoleId,
                       rp => rp.RoleId,
                       (ur, rp) => rp.PermissionId)
                   .Join(
                       _context.Permissions,
                       id => id,
                       p => p.Id,
                       (id, p) => p.Name)
                   .Distinct()
                   .ToList();
            

            var token = _jwtService.GenerateJwt(
               user.Id,
               user.Email,
               user.TenantId,
               false,
               rolesList,
               permissionsList);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Login Successfully",
                data = new
                {
                    accessToken = token.Token,
                    refreshToken = refreshToken.Token,
                    expiresAt = token.Expiry,
                    user = new
                    {
                        user.Id,
                        user.FullName,
                        user.Email,
                        user.TenantId,
                        roles = rolesList,
                        permissions = permissionsList
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

            var user = _context.Users
             .FirstOrDefault(x => x.Id == token.UserId);

            if (user == null)
            {
                return Unauthorized();
            }


            var roles = _context.UserRoles
            .Where(x => x.UserId == user.Id)
            .Select(x => x.Role.Name)
            .Distinct()
            .ToList();

            var permissions = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(_context.Permissions,
                    id => id,
                    p => p.Id,
                    (id, p) => p.Name)
                .Distinct()
                .ToList();

            // ✅ new JWT
            var jwt = _jwtService.GenerateJwt(
                  user.Id,
                  user.Email,
                  user.TenantId,
                  false,
                  roles,
                  permissions);

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