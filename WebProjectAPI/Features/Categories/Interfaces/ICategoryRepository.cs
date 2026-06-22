using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryRepository
    {

        Task<ApiResponse<List<CategoryListDto>>> GetAllAsync(
        PaginationRequest request);
        
        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<bool> DeleteAsync(Category category);

        Task<bool> ChangeStatusAsync(int id);
        Task<List<Category>>
     GetCategoriesAsync();
    }
}