using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.products.Interfaces
{
    public interface IProductRepository
    {
      
        Task<ApiResponse<List<ProductDto>>> GetAllAsync(
         PaginationRequest request);
        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task<bool> DeleteAsync(Product product);

        Task<bool> ChangeStatusAsync(int id);

        IQueryable<Product> GetQueryable();
        Task<Product?> GetBySlugAsync(string slug);

        Task<List<Product>> GetRelatedProductsAsync(
            int categoryId,
            int productId
        );

        Task<List<CategoryWithProductsDto>>GetHomeCategoryProductsAsync();
    }
}