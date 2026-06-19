using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryService
    {

        Task<ApiResponse<List<CategoryDto>>> GetAllAsync(PaginationRequest request);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> GetByIdAsync(int id);

        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);

        Task<ApiResponse<List<CategoryDto>>>
            GetCategoriesAsync();
    }
}
