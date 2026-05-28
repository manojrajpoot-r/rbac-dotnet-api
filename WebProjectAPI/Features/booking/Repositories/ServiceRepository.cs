using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<ApiResponse<List<Service>>> GetAll(
            PaginationRequest request)
        {
            var query = _context.Services
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x =>
                    x.ServiceName.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new ApiResponse<List<Service>>
            {
                Success = true,
                Message = "Service list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        // GET BY ID
        public async Task<ApiResponse<Service>> GetById(int id)
        {
            var data = await _context.Services
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsDeleted);

            return new ApiResponse<Service>
            {
                Success = true,
                Message = "Service fetched successfully",
                Data = data
            };
        }

        // ADD
        public async Task<ApiResponse<Service>> Add(Service model)
        {
            var exists = await _context.Services
                .AnyAsync(x =>
                    x.ServiceName == model.ServiceName &&
                    !x.IsDeleted);

            if (exists)
            {
                return new ApiResponse<Service>
                {
                    Success = false,
                    Message = "Service already exists"
                };
            }

            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;

            await _context.Services.AddAsync(model);

            await _context.SaveChangesAsync();

            return new ApiResponse<Service>
            {
                Success = true,
                Message = "Service added successfully",
                Data = model
            };
        }

        // UPDATE
        public async Task<ApiResponse<Service>> Update(Service model)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(x =>
                    x.Id == model.Id &&
                    !x.IsDeleted);

            if (service == null)
            {
                return new ApiResponse<Service>
                {
                    Success = false,
                    Message = "Service not found"
                };
            }

            service.ServiceName = model.ServiceName;
            service.Price = model.Price;
            service.DurationMinutes =
                model.DurationMinutes;

            service.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ApiResponse<Service>
            {
                Success = true,
                Message = "Service updated successfully",
                Data = service
            };
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(x =>
                    x.Id == id);

            if (service == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Service not found"
                };
            }

            service.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Deleted successfully"
            };
        }

        // STATUS
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(x =>
                    x.Id == id);

            if (service == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Service not found"
                };
            }

            service.IsActive = !service.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status changed successfully"
            };
        }

        // DROPDOWN
        public async Task<List<Service>> Dropdown()
        {
            return await _context.Services
                .Where(x =>
                    x.IsActive &&
                    !x.IsDeleted)
                .ToListAsync();
        }
    }
}