using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Razorpay.Api;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Features.Tenants.Interfaces;
using WebProjectAPI.Models;
namespace WebProjectAPI.Features.Tenants.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        public TenantRepository(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<List<TenantDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.Tenants
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TenantDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subdomain = x.SubDomain,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new ApiResponse<List<TenantDto>>
            {
                Success = true,
                Message = "Tenant list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        // ================= GET BY ID =================
        public async Task<ApiResponse<TenantDto>> GetById(int id)
        {
            var data = await _context.Tenants
                .Where(x => x.Id == id)
                .Select(x => new TenantDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subdomain = x.SubDomain,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<TenantDto>
            {
                Success = true,
                Message = "Tenant fetched successfully",
                Data = data
            };
        }

        // ================= ADD (Tenant + Role + User + UserRole) =================
        public async Task<ApiResponse<TenantDto>> Add(CreateTenantDto model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var exists = await _context.Tenants
                    .AnyAsync(x => x.SubDomain == model.SubDomain);

             
                if (exists)
                {
                    return new ApiResponse<TenantDto>
                    {
                        Success = false,
                        Message = "Subdomain already exists"
                    };
                }

                var emailExists = await _context.Users
                    .AnyAsync(x => x.Email == model.AdminEmail && !x.IsDeleted);

                if (emailExists)
                {
                    return new ApiResponse<TenantDto>
                    {
                        Success = false,
                        Message = "Admin email already exists"
                    };
                }

                // 1. Tenant
                var tenant = new Tenant
                {
                    Name = model.Name,
                    SubDomain = model.SubDomain,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                // 2. Role
                var role = new Role
                {
                    Name = "Tenant Admin",
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = tenant.Id
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                // 3. User
                var user = new User
                {
                    FullName = model.AdminName,
                    Email = model.AdminEmail,
                    TenantId = tenant.Id,
                    IsActive = true,
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

              
                var hasher = _serviceProvider.GetRequiredService<IPasswordHasher<User>>();

                user.PasswordHash = hasher.HashPassword(user, model.Password);



                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 4. UserRole
                var userRole = new UserRole
                {
                    TenantId = tenant.Id,
                    UserId = user.Id,
                    RoleId = role.Id
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<TenantDto>
                {
                    Success = true,
                    Message = "Tenant created successfully",
                    Data = new TenantDto
                    {
                        Id = tenant.Id,
                        Name = tenant.Name,
                        Subdomain = tenant.SubDomain,
                        IsActive = tenant.IsActive,
                        AdminName = user.FullName,
                        AdminEmail = user.Email
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiResponse<TenantDto>
                {
                    Success = false,
                    Message = ex.InnerException?.Message,
    
                };
            }

        }

        // ================= UPDATE =================
        public async Task<ApiResponse<TenantDto>> Update(UpdateTenantDto model)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (tenant == null)
            {
                return new ApiResponse<TenantDto>
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            tenant.Name = model.Name;
            tenant.SubDomain = model.SubDomain;
            tenant.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<TenantDto>
            {
                Success = true,
                Message = "Tenant updated successfully"
            };
        }

        // ================= DELETE (SOFT DELETE) =================
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }


            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Tenant deleted successfully"
            };
        }

        // ================= CHANGE STATUS =================
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);

            if (tenant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            tenant.IsActive = !tenant.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        // ================= DROPDOWN =================
        public async Task<List<TenantDropdownDto>> Dropdown()
        {
            return await _context.Tenants
                .Where(x => x.IsActive)
                .Select(x => new TenantDropdownDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}