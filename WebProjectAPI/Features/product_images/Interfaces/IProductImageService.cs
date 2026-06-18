using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;

namespace WebProjectAPI.Features.product_images.Interfaces
{
    public interface IProductImageService
    {
        Task<ApiResponse<List<ProductImageDto>>> GetAllAsync(int productId);
        Task<ApiResponse<List<ProductImageDto>>> CreateAsync(
            int productId,
            List<IFormFile> images);

        Task<ApiResponse<ProductImageDto>> UpdateAsync(
            int id,
            ProductImageUpdateDto dto);

        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}