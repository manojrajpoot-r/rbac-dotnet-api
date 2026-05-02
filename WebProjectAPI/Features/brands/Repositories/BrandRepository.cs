using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.Interfaces;
using WebProjectAPI.Features.brands.Models;



namespace WebProjectAPI.Features.brands.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Brand> Brands, int TotalRecords)> GetAllAsync(
              int pageNumber,
              int pageSize,
              string search)
        {
            var query = _context.Brands.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            int totalRecords = await query.CountAsync();

            var brands = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (brands, totalRecords);
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            _context.Brands.Add(brand);

            await _context.SaveChangesAsync();

            return brand;
        }

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);

            await _context.SaveChangesAsync();

            return brand;
        }

        public async Task<bool> DeleteAsync(Brand brand)
        {
            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                return false;

            brand.Status = !brand.Status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}