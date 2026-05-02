using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Models;


namespace WebProjectAPI.Features.products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Product> Products, int TotalRecords)> GetAllAsync(
              int pageNumber,
              int pageSize,
              string search)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            int totalRecords = await query.CountAsync();

            var products = await query
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();

                    return (products, totalRecords);

            
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            product.Status = !product.Status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}