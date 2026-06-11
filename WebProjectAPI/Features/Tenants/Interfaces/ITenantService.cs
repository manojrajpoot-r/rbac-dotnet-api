using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;

namespace WebProjectAPI.Features.Tenants.Interfaces
{
    public interface ITenantService
    {
        Task<ApiResponse<List<TenantDto>>> GetAll(PaginationRequest request);

        Task<ApiResponse<TenantDto>> GetById(int id);

        Task<ApiResponse<TenantDto>> Add(CreateTenantDto model);

        Task<ApiResponse<TenantDto>> Update(UpdateTenantDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<ApiResponse<List<TenantDropdownDto>>> Dropdown();
    }
}