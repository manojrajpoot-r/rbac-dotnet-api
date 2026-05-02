using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.sub_categories.Models;

namespace WebProjectAPI.Features.sub_categories.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<List<SubCategory>> GetAllAsync();

        Task<SubCategory?> GetByIdAsync(int id);

        Task<SubCategory> CreateAsync(SubCategory category);

        Task<SubCategory> UpdateAsync(SubCategory category);

        Task<bool> DeleteAsync(SubCategory category);

        Task<bool> ChangeStatusAsync(int id);
    }
}
