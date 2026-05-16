using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryRepository
    {

        Task<(List<Category> Data, int TotalRecords)>GetAllAsync(int pageNumber,int pageSize, string search);
        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<bool> DeleteAsync(Category category);

        Task<bool> ChangeStatusAsync(int id);
        Task<List<Category>>
     GetCategoriesAsync();
    }
}