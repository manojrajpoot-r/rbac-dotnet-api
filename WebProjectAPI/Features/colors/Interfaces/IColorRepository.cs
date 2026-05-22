using WebProjectAPI.Features.colors.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;

namespace WebProjectAPI.Features.colors.Interfaces
{
    public interface IColorRepository
    {
        Task<ApiResponse<List<ColorDto>>> GetAll(PaginationRequest request);

        Task<ApiResponse<ColorDto>> GetById(int id);

        Task<ApiResponse<ColorDto>> Add(ColorDto model);

        Task<ApiResponse<ColorDto>> Update(ColorDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<List<ColorDto>> Dropdown();
       
    }
}
