using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
namespace WebProjectAPI.Features.colors.Interfaces
{
    public interface IColorService
    {
        Task<ApiResponse<List<ColorDto>>> GetAll(PaginationRequest request);

        Task<ApiResponse<ColorDto>> GetById(int id);

        Task<ApiResponse<ColorDto>> Add(ColorDto model);

        Task<ApiResponse<ColorDto>> Update(int id,ColorDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<bool>> ChangeStatusAsync(int id);

        Task<ApiResponse<List<ColorDto>>> Dropdown();
    }
}
