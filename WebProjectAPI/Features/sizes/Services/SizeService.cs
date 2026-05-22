using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sizes.DTOs;
using WebProjectAPI.Features.sizes.Interfaces;

namespace WebProjectAPI.Features.sizes.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _repository;

        public SizeService(ISizeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<SizeDto>>> GetAll(PaginationRequest request)
        {
            return await _repository.GetAll(request);
        }

        public async Task<ApiResponse<SizeDto>> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<ApiResponse<SizeDto>> Add(SizeDto model)
        {
            return await _repository.Add(model);
        }

        public async Task<ApiResponse<SizeDto>> Update(SizeDto model)
        {
            return await _repository.Update(model);
        }

        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _repository.ChangeStatus(id);
        }


        public async Task<ApiResponse<List<SizeDto>>> Dropdown()
        {
            var data = await _repository.Dropdown();

            return new ApiResponse<List<SizeDto>>
            {
                Success = true,
                Message = "Size dropdown fetched successfully",
                Data = data
            };
        
   
        }
    }
}
