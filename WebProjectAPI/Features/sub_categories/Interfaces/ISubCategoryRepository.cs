using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sub_categories.DTOs;
using WebProjectAPI.Features.sub_categories.Models;

namespace WebProjectAPI.Features.sub_categories.Interfaces
{
    public interface ISubCategoryRepository
    {
        
        
            Task<ApiResponse<List<SubCategoryListDto>>> GetAllAsync(
                PaginationRequest request);
        
        Task<SubCategory?> GetByIdAsync(int id);

        Task<SubCategory> CreateAsync(SubCategory category);

        Task<SubCategory> UpdateAsync(SubCategory category);

        Task<bool> DeleteAsync(SubCategory category);

        Task<bool> ChangeStatusAsync(int id);
        Task<List<SubCategory>> GetAllSubCategoriesAsync();
    }
}
