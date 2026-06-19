using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.plans.DTOs;
using WebProjectAPI.Features.plans.Models;

namespace WebProjectAPI.Features.plans.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _context;

        public PlanRepository(AppDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<List<PlanDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.Plans.AsQueryable();

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search));
            }

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PlanDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DurationInMonths = x.DurationInMonths,
                    MaxUsers = x.MaxUsers,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new ApiResponse<List<PlanDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        // ================= GET BY ID =================
        public async Task<ApiResponse<PlanDto>> GetById(int id)
        {
            var data = await _context.Plans
                .Where(x => x.Id == id)
                .Select(x => new PlanDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DurationInMonths = x.DurationInMonths,
                    MaxUsers = x.MaxUsers,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<PlanDto>
            {
                Success = true,
                Message = "Plan fetched successfully",
                Data = data
            };
        }

        // ================= ADD =================
        public async Task<ApiResponse<PlanDto>> Add(CreatePlanDto model)
        {
            var plan = new Plan
            {
                Name = model.Name,
                Price = model.Price,
                DurationInMonths = model.DurationInMonths,
                MaxUsers = model.MaxUsers,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();

            return new ApiResponse<PlanDto>
            {
                Success = true,
                Message = "Plan created successfully",
                Data = new PlanDto
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Price = plan.Price,
                    DurationInMonths = plan.DurationInMonths,
                    MaxUsers = plan.MaxUsers,
                    IsActive = plan.IsActive
                }
            };
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<PlanDto>> Update(UpdatePlanDto model)
        {
            var plan = await _context.Plans
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (plan == null)
            {
                return new ApiResponse<PlanDto>
                {
                    Success = false,
                    Message = "Plan not found"
                };
            }

            plan.Name = model.Name;
            plan.Price = model.Price;
            plan.DurationInMonths = model.DurationInMonths;
            plan.MaxUsers = model.MaxUsers;
            plan.IsActive = model.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ApiResponse<PlanDto>
            {
                Success = true,
                Message = "Plan updated successfully"
            };
        }

        // ================= DELETE =================
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Plan not found"
                };
            }

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Plan deleted successfully"
            };
        }

        // ================= CHANGE STATUS =================
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Plan not found"
                };
            }

            plan.IsActive = !plan.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        // ================= DROPDOWN =================
        public async Task<List<PlanDropdownDto>> Dropdown()
        {
            return await _context.Plans
                .Where(x => x.IsActive)
                .Select(x => new PlanDropdownDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}