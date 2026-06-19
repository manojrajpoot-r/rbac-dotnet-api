using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.plans.DTOs;

namespace WebProjectAPI.Features.plans.Repositories
{
    public interface IPlanRepository
    {
        Task<ApiResponse<List<PlanDto>>> GetAll(PaginationRequest request);
        Task<ApiResponse<PlanDto>> GetById(int id);
        Task<ApiResponse<PlanDto>> Add(CreateplanDto model);
        Task<ApiResponse<PlanDto>> Update(UpdatePlanDto model);
        Task<ApiResponse<string>> Delete(int id);
        Task<ApiResponse<string>> ChangeStatus(int id);
        Task<List<PlanDropdownDto>> Dropdown();
    }
}