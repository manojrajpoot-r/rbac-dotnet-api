using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.Categories.Interfaces
{
    public interface ICategoryRepository
    {

        Task<(List<Category> Data, int TotalRecords)>GetAllAsync(PaginationRequest request);
        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task<bool> DeleteAsync(Category category);

        Task<bool> ChangeStatusAsync(int id);
        Task<List<Category>>
     GetCategoriesAsync();
    }
}