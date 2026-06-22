using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sizes.DTOs;
using WebProjectAPI.Features.sizes.Interfaces;
using WebProjectAPI.Features.sizes.Models;
using WebProjectAPI.Services.Interfaces;
namespace WebProjectAPI.Features.sizes.Repositories
{
    public class SizeRepository : ISizeRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public SizeRepository(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // LIST WITH PAGINATION
        public async Task<ApiResponse<List<SizeDto>>> GetAll(
          PaginationRequest request)
        {
            var query = _context.Sizes
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new SizeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return new ApiResponse<List<SizeDto>>
            {
                Success = true,
                Message = "Size list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        // GET BY ID
        public async Task<ApiResponse<SizeDto>> GetById(int id)
        {
            var query = _context.Sizes
                .Where(x =>
                    x.Id == id &&
                    !x.IsDeleted);

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            var data = await query
                .Select(x => new SizeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<SizeDto>
            {
                Success = true,
                Message = "Size fetched successfully",
                Data = data
            };
        }

        // ADD
        public async Task<ApiResponse<SizeDto>> Add(SizeDto model)
        {
            var exists = await _context.Sizes
                .AnyAsync(x =>
                    x.Name == model.Name &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId);

            if (exists)
            {
                return new ApiResponse<SizeDto>
                {
                    Success = false,
                    Message = "Size already exists"
                };
            }

            var size = new Size
            {
                TenantId = _currentUser.TenantId.Value,
                Name = model.Name,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _context.Sizes.Add(size);

            await _context.SaveChangesAsync();

            model.Id = size.Id;

            return new ApiResponse<SizeDto>
            {
                Success = true,
                Message = "Size added successfully",
                Data = model
            };
        }

        // UPDATE
        public async Task<ApiResponse<SizeDto>> Update(SizeDto model)
        {
            var size = await _context.Sizes
                .FirstOrDefaultAsync(x =>
                    x.Id == model.Id &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId);

            if (size == null)
            {
                return new ApiResponse<SizeDto>
                {
                    Success = false,
                    Message = "Size not found"
                };
            }

            size.Name = model.Name;
            size.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ApiResponse<SizeDto>
            {
                Success = true,
                Message = "Size updated successfully",
                Data = model
            };
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var size = await _context.Sizes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (size == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Size not found"
                };
            }

            size.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Size deleted successfully"
            };
        }

        // STATUS CHANGE
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var size = await _context.Sizes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (size == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Size not found"
                };
            }

            size.IsActive = !size.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        // DROPDOWN
        public async Task<List<SizeDto>> Dropdown()
        {
            return await _context.Sizes
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId)
                .Select(x => new SizeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}
