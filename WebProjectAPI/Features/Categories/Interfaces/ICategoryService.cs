using WebProjectAPI.Features.Categories.DTOs;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();

        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> GetByIdAsync(int id);

        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);
    }
}
