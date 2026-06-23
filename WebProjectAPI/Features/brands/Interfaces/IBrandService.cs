using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;


namespace WebProjectAPI.Features.brands.Interfaces
{
    public interface IBrandService
    {
        Task<ApiResponse<List<BrandListDto>>> GetAllAsync(
         PaginationRequest request);
        Task<ApiResponse<BrandListDto>> GetByIdAsync(int id);

        Task<ApiResponse<BrandListDto>> CreateAsync(CreateBrandDto dto);

        Task<ApiResponse<BrandListDto>> UpdateAsync(int id, UpdateBrandDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);

        Task<ApiResponse<bool>> ChangeStatusAsync(int id);
        Task<ApiResponse<List<BrandListDto>>>GetAllBrandsAsync();
    }
}