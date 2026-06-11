using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Features.Tenants.Interfaces;

namespace WebProjectAPI.Features.Tenants.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<ApiResponse<List<TenantDto>>> GetAll(PaginationRequest request)
        {
            return await _tenantRepository.GetAll(request);
        }

        public async Task<ApiResponse<TenantDto>> GetById(int id)
        {
            return await _tenantRepository.GetById(id);
        }

        public async Task<ApiResponse<TenantDto>> Add(CreateTenantDto model)
        {
            return await _tenantRepository.Add(model);
        }

        public async Task<ApiResponse<TenantDto>> Update(UpdateTenantDto model)
        {
            return await _tenantRepository.Update(model);
        }

        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await _tenantRepository.Delete(id);
        }

        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _tenantRepository.ChangeStatus(id);
        }

        public async Task<ApiResponse<List<TenantDropdownDto>>> Dropdown()
        {
            var data = await _tenantRepository.Dropdown();

            return new ApiResponse<List<TenantDropdownDto>>
            {
                Success = true,
                Message = "Tenant dropdown fetched successfully",
                Data = data
            };
        }
    }
}