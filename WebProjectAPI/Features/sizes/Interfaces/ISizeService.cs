using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sizes.DTOs;

namespace WebProjectAPI.Features.sizes.Interfaces
{
    public interface ISizeService
    {
        Task<ApiResponse<List<SizeDto>>> GetAll(PaginationRequest request);

        Task<ApiResponse<SizeDto>> GetById(int id);

        Task<ApiResponse<SizeDto>> Add(SizeDto model);

        Task<ApiResponse<SizeDto>> Update(SizeDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<ApiResponse<List<SizeDto>>> Dropdown();
    }
}
