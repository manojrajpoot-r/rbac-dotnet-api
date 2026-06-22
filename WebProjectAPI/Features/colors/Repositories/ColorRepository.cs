using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Features.colors.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Services.Interfaces;
namespace WebProjectAPI.Features.colors.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ColorRepository(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // LIST WITH PAGINATION
        public async Task<ApiResponse<List<ColorDto>>> GetAll(
         PaginationRequest request)
        {
            var query = _context.Colors
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
                .Select(x => new ColorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToListAsync();

            return new ApiResponse<List<ColorDto>>
            {
                Success = true,
                Message = "Color list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }
        // GET BY ID
        public async Task<ApiResponse<ColorDto>> GetById(int id)
        {
            var query = _context.Colors
                .Where(x =>
                    x.Id == id &&
                    !x.IsDeleted);

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            var data = await query
                .Select(x => new ColorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<ColorDto>
            {
                Success = true,
                Message = "Color fetched successfully",
                Data = data
            };
        }

        // ADD
        public async Task<ApiResponse<ColorDto>> Add(ColorDto model)
        {
            var exists = await _context.Colors
                .AnyAsync(x =>
                    x.Name == model.Name &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId);

            if (exists)
            {
                return new ApiResponse<ColorDto>
                {
                    Success = false,
                    Message = "Color already exists"
                };
            }

            var color = new Color
            {
                TenantId = _currentUser.TenantId.Value,
                Name = model.Name,
                Code = model.Code,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _context.Colors.Add(color);

            await _context.SaveChangesAsync();

            model.Id = color.Id;

            return new ApiResponse<ColorDto>
            {
                Success = true,
                Message = "Color added successfully",
                Data = model
            };
        }

        // UPDATE
        public async Task<ApiResponse<ColorDto>> Update(ColorDto model)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(x =>
                    x.Id == model.Id &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId);

            if (color == null)
            {
                return new ApiResponse<ColorDto>
                {
                    Success = false,
                    Message = "Color not found"
                };
            }

            color.Name = model.Name;
            color.Code = model.Code;
            color.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ApiResponse<ColorDto>
            {
                Success = true,
                Message = "Color updated successfully",
                Data = model
            };
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);

            if (color == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Color not found"
                };
            }

            color.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Color deleted successfully"
            };
        }

        // STATUS CHANGE
        public async Task<List<ColorDto>> Dropdown()
        {
            return await _context.Colors
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted &&
                    x.TenantId == _currentUser.TenantId)
                .Select(x => new ColorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToListAsync();
        }
        
       
    }

}
