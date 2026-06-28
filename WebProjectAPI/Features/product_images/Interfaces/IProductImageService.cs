using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.product_images.DTOs;

namespace WebProjectAPI.Features.product_images.Interfaces
{

    public interface IProductImageService
    {
        Task<ApiResponse<List<ProductImageDto>>> GetAllAsync(int productId);

        Task<ApiResponse<List<ProductImageDto>>> CreateAsync(
            int productId,
            List<IFormFile> images);

        Task<ApiResponse<string>> DeleteAsync(int id);

        Task<ApiResponse<string>> SetPrimaryAsync(int id);
    }
}