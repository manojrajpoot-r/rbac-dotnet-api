using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebProjectAPI.Features.Common.ApiResponse;

namespace WebProjectAPI.Repositories.Implementations
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public UserRepository(AppDbContext context,ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<List<UserListDto>>> GetAll(PaginationRequest request)
        {
           
            var query = _context.Users
            .IgnoreQueryFilters()
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
                    x.FullName.Contains(request.Search) ||
                    x.Email.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserListDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    Status = x.Status,
                    TenantName = x.Tenant != null
                    ? x.Tenant.Name
                    : "Platform"
                        })
                .ToListAsync();

            return new ApiResponse<List<UserListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }



        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User Add(User user)
        {
            if (!_currentUser.TenantId.HasValue)
                throw new Exception("TenantId not found in token");

            user.TenantId = _currentUser.TenantId.Value;
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            var existing = _context.Users.Find(user.Id);
            if (existing == null) return null;

            existing.FullName = user.FullName ?? existing.FullName;
          

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool ToggleStatus(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            user.Status = user.Status==1?0:1;
            _context.SaveChanges();
            return true;
        }
    }
}


