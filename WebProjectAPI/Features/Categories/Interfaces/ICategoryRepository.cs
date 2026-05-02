using WebProjectAPI.Features.Categories.Models;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<bool> DeleteAsync(Category category);

        Task<bool> ChangeStatusAsync(int id);
    }
}