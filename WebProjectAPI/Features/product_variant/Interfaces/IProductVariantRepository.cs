using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_variant.DTOs;
using WebProjectAPI.Features.sizes.DTOs;

namespace WebProjectAPI.Features.product_variant.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<ApiResponse<List<ProductVariantDto>>> GetAll(PaginationRequest request);

        Task<ApiResponse<ProductVariantDto>> GetById(int id);

        Task<ApiResponse<ProductVariantCreateUpdateDto>> Add(ProductVariantCreateUpdateDto model);

        Task<ApiResponse<ProductVariantCreateUpdateDto>> Update(int id, ProductVariantCreateUpdateDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);
        
        Task<List<ProductVariantDto>> GetByProductId(int productId);

      
    }
}
