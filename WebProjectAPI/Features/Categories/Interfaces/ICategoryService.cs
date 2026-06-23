using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;


namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryService
    {

        Task<ApiResponse<List<CategoryListDto>>> GetAllAsync(PaginationRequest request);
        Task<CategoryListDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryListDto> GetByIdAsync(int id);

        Task<CategoryListDto> UpdateAsync(int id, UpdateCategoryDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);

        Task<ApiResponse<List<CategoryListDto>>>GetCategoriesAsync();
    }
}
