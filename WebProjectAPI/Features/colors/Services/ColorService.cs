using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
namespace WebProjectAPI.Features.colors.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _repository;

        public ColorService(IColorRepository repository)
        {
            _repository = repository;
        }

        // LIST
        public async Task<ApiResponse<List<ColorDto>>> GetAll(PaginationRequest request)
        {
            return await _repository.GetAll(request);
        }

        // GET BY ID
        public async Task<ApiResponse<ColorDto>> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        // ADD
        public async Task<ApiResponse<ColorDto>> Add(ColorDto model)
        {
            return await _repository.Add(model);
        }

        // UPDATE
        public async Task<ApiResponse<ColorDto>> Update(ColorDto model)
        {
            return await _repository.Update(model);
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        // STATUS CHANGE
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _repository.ChangeStatus(id);
        }

        // forntend
        public async Task<ApiResponse<List<ColorDto>>> Dropdown()
        {
            var data = await _repository.Dropdown();

            return new ApiResponse<List<ColorDto>>
            {
                Success = true,
                Message = "Color dropdown fetched successfully",
                Data = data
            };
        }
    }
}
