using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.subscription.DTOs;
using WebProjectAPI.Features.subscription.Interfaces;
using WebProjectAPI.Features.subscription.Repositories;

namespace WebProjectAPI.Features.subscription.Services
{
    public class TenantSubscriptionService : ITenantSubscriptionService
    {
        private readonly ITenantSubscriptionRepository _repo;

        public TenantSubscriptionService(ITenantSubscriptionRepository repo)
        {
            _repo = repo;
        }

        public Task<ApiResponse<List<TenantSubscriptionDto>>> GetAll(PaginationRequest request)
            => _repo.GetAll(request);

        public Task<ApiResponse<TenantSubscriptionDto>> GetById(int id)
            => _repo.GetById(id);

        public Task<ApiResponse<TenantSubscriptionDto>> Add(CreateTenantSubscriptionDto model)
            => _repo.Add(model);

        public Task<ApiResponse<TenantSubscriptionDto>> Update(UpdateTenantSubscriptionDto model)
            => _repo.Update(model);

        public Task<ApiResponse<string>> Delete(int id)
            => _repo.Delete(id);

        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _repo.ChangeStatus(id);
        }

        public async Task<ApiResponse<string>> RenewSubscription(int id)
        {
            return await _repo.RenewSubscription(id);
        }

      


    }
}
