using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;


namespace WebProjectAPI.Features.brands.Interfaces
{
    public interface IBrandRepository
    {
        Task<(List<Brand> Brands, int TotalRecords)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string search);
        Task<Brand?> GetByIdAsync(int id);

        Task<Brand> CreateAsync(Brand brand);

        Task<Brand> UpdateAsync(Brand brand);

        Task<bool> DeleteAsync(Brand brand);

        Task<bool> ChangeStatusAsync(int id);
    }
}