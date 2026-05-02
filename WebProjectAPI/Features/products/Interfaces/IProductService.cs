using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Helpers;


namespace WebProjectAPI.Features.products.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductDto>>> GetAllAsync(
       int pageNumber,
       int pageSize,
       string search);

        Task<ApiResponse<ProductDto>> GetByIdAsync(int id);

        Task<ProductDto> CreateAsync(CreateProductDto dto);

        Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ChangeStatusAsync(int id);
    }
}