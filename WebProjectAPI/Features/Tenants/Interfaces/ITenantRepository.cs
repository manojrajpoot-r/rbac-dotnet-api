using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;

public interface ITenantRepository
{
    Task<ApiResponse<List<TenantDto>>> GetAll(PaginationRequest request);

    Task<ApiResponse<TenantDto>> GetById(int id);

    Task<ApiResponse<TenantDto>> Add(CreateTenantDto model);

    Task<ApiResponse<TenantDto>> Update(UpdateTenantDto model);

    Task<ApiResponse<string>> Delete(int id);

    Task<ApiResponse<string>> ChangeStatus(int id);

    Task<List<TenantDropdownDto>> Dropdown();
}