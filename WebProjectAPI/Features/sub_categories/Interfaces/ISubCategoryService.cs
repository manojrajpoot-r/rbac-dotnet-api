using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sub_categories.DTOs;

namespace WebProjectAPI.Features.sub_categories.Interfaces
{
    public interface ISubCategoryService
    {

        Task<ApiResponse<List<SubCategoryListDto>>> GetAllAsync(
         PaginationRequest request);
        Task<SubCategoryListDto> CreateAsync(CreateSubCategoryDto dto);
        Task<SubCategoryListDto> GetByIdAsync(int id);

        Task<SubCategoryListDto> UpdateAsync(int id, UpdateSubCategoryDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);

        Task<List<SubCategoryListDto>> GetAllSubCategoriesAsync();
    }
}
