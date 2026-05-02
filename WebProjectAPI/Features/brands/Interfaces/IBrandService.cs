using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Helpers;


namespace WebProjectAPI.Features.brands.Interfaces
{
    public interface IBrandService
    {
        Task<ApiResponse<List<BrandDto>>> GetAllAsync(int pageNumber, int pageSize, string search);

        Task<ApiResponse<BrandDto>> GetByIdAsync(int id);

        Task<ApiResponse<BrandDto>> CreateAsync(CreateBrandDto dto);

        Task<ApiResponse<BrandDto>> UpdateAsync(int id, UpdateBrandDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);

        Task<ApiResponse<bool>> ChangeStatusAsync(int id);
    }
}