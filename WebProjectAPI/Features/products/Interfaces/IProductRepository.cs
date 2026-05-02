using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.products.Interfaces
{
    public interface IProductRepository
    {
        Task<(List<Product> Products, int TotalRecords)> GetAllAsync(
           int pageNumber,
           int pageSize,
           string search);

        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task<bool> DeleteAsync(Product product);

        Task<bool> ChangeStatusAsync(int id);
    }
}