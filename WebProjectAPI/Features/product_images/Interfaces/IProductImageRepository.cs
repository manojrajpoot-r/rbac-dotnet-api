using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_images.Models;

namespace WebProjectAPI.Features.product_images.Interfaces
{
 
    public interface IProductImageRepository
    {
        Task<(List<ProductImage>, int)> GetAllAsync(int productId);

        Task<ProductImage?> GetByIdAsync(int id);

        Task<List<ProductImage>> CreateRangeAsync(List<ProductImage> images);

        Task DeleteAsync(ProductImage image);

        Task SetPrimaryAsync(ProductImage image);
    }
}