using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;


namespace WebProjectAPI.Features.brands.Interfaces
{
    public interface IBrandRepository
    {
        Task<ApiResponse<List<BrandListDto>>> GetAllAsync(
        PaginationRequest request);
        Task<Brand?> GetByIdAsync(int id);
        
        Task<Brand> CreateAsync(Brand brand);

        Task<Brand> UpdateAsync(Brand brand);

        Task<bool> DeleteAsync(Brand brand);

        Task<bool> ChangeStatusAsync(int id);


        Task<List<Brand>> GetAllBrandsAsync();
    }
}