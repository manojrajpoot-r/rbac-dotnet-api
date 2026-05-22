using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_variant.DTOs;
using WebProjectAPI.Features.product_variant.Interfaces;

namespace WebProjectAPI.Features.product_variant.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _repository;

        public ProductVariantService(IProductVariantRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<ProductVariantDto>>> GetAll(PaginationRequest request)
        {
            return await _repository.GetAll(request);
        }

        public async Task<ApiResponse<ProductVariantDto>> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<ApiResponse<ProductVariantCreateUpdateDto>> Add(ProductVariantCreateUpdateDto model)
        {
            model.SKU = SKUHelper.GenerateSKU("PRD");

            return await _repository.Add(model);
        }
        public async Task<ApiResponse<ProductVariantCreateUpdateDto>> Update(ProductVariantCreateUpdateDto model)
        {
            return await _repository.Update(model);
        }

        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            return await _repository.ChangeStatus(id);
        }

        public async Task<List<ProductVariantDto>> GetByProductId(int productId)
        {
            return await _repository.GetByProductId(productId);
        }
    }
}
