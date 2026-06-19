


using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.plans.DTOs;
using WebProjectAPI.Features.plans.Models;
using WebProjectAPI.Features.subscription.DTOs;
using WebProjectAPI.Features.subscription.Interfaces;
using WebProjectAPI.Features.subscription.Models;
namespace WebProjectAPI.Features.subscription.Repositories
{
    public class TenantSubscriptionRepository : ITenantSubscriptionRepository
    {
        private readonly AppDbContext _context;

        public TenantSubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<List<TenantSubscriptionDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.TenantSubscriptions.AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TenantSubscriptionDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    PlanId = x.PlanId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Amount = x.Amount,
                    SubscriptionStatus = x.SubscriptionStatus
                })
                .ToListAsync();

            return new ApiResponse<List<TenantSubscriptionDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        // ================= GET BY ID =================
        public async Task<ApiResponse<TenantSubscriptionDto>> GetById(int id)
        {
            var data = await _context.TenantSubscriptions
                .Where(x => x.Id == id)
                .Select(x => new TenantSubscriptionDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    PlanId = x.PlanId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Amount = x.Amount,
                    SubscriptionStatus = x.SubscriptionStatus
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<TenantSubscriptionDto>
            {
                Success = true,
                Data = data
            };
        }

        // ================= ADD =================
        public async Task<ApiResponse<TenantSubscriptionDto>> Add(CreateTenantSubscriptionDto model)
        {
            var plan = await _context.Plans.FindAsync(model.PlanId);

            if (plan == null)
            {
                return new ApiResponse<TenantSubscriptionDto>
                {
                    Success = false,
                    Message = "Plan not found"
                };
            }

            var sub = new TenantSubscription
            {
                TenantId = model.TenantId,
                PlanId = model.PlanId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(plan.DurationInMonths),
                Amount = plan.Price,
                SubscriptionStatus = "Active"
            };

            _context.TenantSubscriptions.Add(sub);
            await _context.SaveChangesAsync();

            return new ApiResponse<TenantSubscriptionDto>
            {
                Success = true,
                Message = "Subscription created successfully"
            };
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<TenantSubscriptionDto>> Update(UpdateTenantSubscriptionDto model)
        {
            var sub = await _context.TenantSubscriptions.FindAsync(model.Id);

            if (sub == null)
            {
                return new ApiResponse<TenantSubscriptionDto>
                {
                    Success = false,
                    Message = "Subscription not found"
                };
            }

            sub.SubscriptionStatus = model.SubscriptionStatus;

            await _context.SaveChangesAsync();

            return new ApiResponse<TenantSubscriptionDto>
            {
                Success = true,
                Message = "Updated successfully"
            };
        }

        // ================= DELETE =================
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var sub = await _context.TenantSubscriptions.FindAsync(id);

            if (sub == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Not found"
                };
            }

            _context.TenantSubscriptions.Remove(sub);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Deleted successfully"
            };
        }
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var sub = await _context.TenantSubscriptions.FindAsync(id);

            if (sub == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Subscription not found"
                };
            }

            sub.SubscriptionStatus =
                sub.SubscriptionStatus == "Active"
                ? "Inactive"
                : "Active";

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }
    }
       
}