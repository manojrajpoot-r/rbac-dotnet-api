using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.plans.DTOs;
using WebProjectAPI.Features.plans.Interfaces;
using WebProjectAPI.Features.plans.Repositories;

namespace WebProjectAPI.Features.plans.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;

        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<List<PlanDto>>> GetAll(PaginationRequest request)
        {
            return await _planRepository.GetAll(request);
        }

        // ================= GET BY ID =================
        public async Task<ApiResponse<PlanDto>> GetById(int id)
        {
            return await _planRepository.GetById(id);
        }

        // ================= ADD =================
        public async Task<ApiResponse<PlanDto>> Add(CreateplanDto model)
        {
            return await _planRepository.Add(model);
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<PlanDto>> Update(UpdatePlanDto model)
        {
            return await _planRepository.Update(model);
        }

        // ================= DELETE =================
        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await _planRepository.Delete(id);
        }

        // ================= CHANGE STATUS =================
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _planRepository.ChangeStatus(id);
        }

        // ================= DROPDOWN =================
        public async Task<List<PlanDropdownDto>> Dropdown()
        {
            return await _planRepository.Dropdown();
        }
    }
}