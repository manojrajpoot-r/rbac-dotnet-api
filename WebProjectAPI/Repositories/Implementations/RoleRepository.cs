using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public RoleRepository(AppDbContext context,ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<List<RoleListDto>>> GetAll(
            PaginationRequest request)
        {
            var query = _context.Roles
                .Include(x => x.Tenant)
                .AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new RoleListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status,
                    TenantName = x.Tenant != null
                        ? x.Tenant.Name
                        : "Platform"
                })
                .ToListAsync();

            return new ApiResponse<List<RoleListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }


        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role Add(Role role)
        {
            if (!_currentUser.TenantId.HasValue)
                throw new Exception("TenantId not found in token");

            role.TenantId = _currentUser.TenantId.Value;
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public Role Update(Role role)
        {
            var existing = _context.Roles.Find(role.Id);
            if (existing == null) return null;

            existing.Name = role.Name;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }

        public bool ToggleStatus(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null) return false;

            role.Status = role.Status == 1 ? 0 : 1;
            _context.SaveChanges();
            return true;
        }
    }
}
