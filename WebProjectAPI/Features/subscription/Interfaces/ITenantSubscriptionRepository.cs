using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.subscription.DTOs;

namespace WebProjectAPI.Features.subscription.Interfaces
{
    public interface ITenantSubscriptionRepository
    {
        Task<ApiResponse<List<TenantSubscriptionDto>>> GetAll(PaginationRequest request);
        Task<ApiResponse<TenantSubscriptionDto>> GetById(int id);
        Task<ApiResponse<TenantSubscriptionDto>> Add(CreateTenantSubscriptionDto model);
        Task<ApiResponse<TenantSubscriptionDto>> Update(UpdateTenantSubscriptionDto model);
        Task<ApiResponse<string>> Delete(int id);
        Task<ApiResponse<string>> ChangeStatus(int id);
        Task<ApiResponse<string>> RenewSubscription(int id);
        
    }
}
