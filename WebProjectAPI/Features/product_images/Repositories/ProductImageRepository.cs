using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.product_images.Interfaces;
using WebProjectAPI.Features.product_images.Models;

namespace WebProjectAPI.Features.product_images.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<ProductImage>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string search)
        {
            var query = _context.ProductImages
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.ImageUrl.Contains(search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }

        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ProductImage>> CreateRangeAsync(
            List<ProductImage> images)
        {
            await _context.ProductImages
                .AddRangeAsync(images);

            await _context.SaveChangesAsync();

            return images;
        }

        public async Task UpdateAsync(ProductImage image)
        {
            _context.ProductImages.Update(image);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductImage image)
        {
            _context.ProductImages.Remove(image);

            await _context.SaveChangesAsync();
        }
    }
}