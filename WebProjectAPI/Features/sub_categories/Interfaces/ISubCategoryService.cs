using WebProjectAPI.Features.sub_categories.DTOs;

namespace WebProjectAPI.Features.sub_categories.Interfaces
{
    public interface ISubCategoryService
    {
        Task<List<SubCategoryDto>> GetAllAsync();

        Task<SubCategoryDto> CreateAsync(CreateSubCategoryDto dto);
        Task<SubCategoryDto> GetByIdAsync(int id);

        Task<SubCategoryDto> UpdateAsync(int id, UpdateSubCategoryDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);
    }
}
