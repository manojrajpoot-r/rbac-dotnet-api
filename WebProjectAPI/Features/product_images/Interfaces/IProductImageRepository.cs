using WebProjectAPI.Features.product_images.Models;

namespace WebProjectAPI.Features.product_images.Interfaces
{
    public interface IProductImageRepository
    {
        Task<(List<ProductImage>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string search);

        Task<ProductImage?> GetByIdAsync(int id);

        Task<List<ProductImage>> CreateRangeAsync(List<ProductImage> images);

        Task UpdateAsync(ProductImage image);

        Task DeleteAsync(ProductImage image);
    }
}