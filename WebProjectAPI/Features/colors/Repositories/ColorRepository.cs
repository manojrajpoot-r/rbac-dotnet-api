using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.colors.Models;
namespace WebProjectAPI.Features.colors.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly AppDbContext _context;

        public ColorRepository(AppDbContext context)
        {
            _context = context;
        }

        // LIST WITH PAGINATION
        public async Task<ApiResponse<List<ColorDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.Colors
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            // SEARCH
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
            var data = await _context.Colors
                .Where(x => x.Id == id && !x.IsDeleted)
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
                    !x.IsDeleted);

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
                    !x.IsDeleted);

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
            var color = await _context.Colors.FindAsync(id);

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
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Color not found"
                };
            }

            color.IsActive = !color.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        // DROPDOWN
        public async Task<List<ColorDto>> Dropdown()
        {
            return await _context.Colors
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted)
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
