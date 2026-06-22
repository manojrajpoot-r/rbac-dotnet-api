using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_images.Interfaces;
using WebProjectAPI.Features.product_images.Models;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Features.product_images.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ProductImageRepository(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<(List<ProductImage>, int)> GetAllAsync(int productId)
        {
            var query = _context.ProductImages
                .Include(x => x.Product)
                .Where(x =>
                    x.ProductId == productId &&
                    x.TenantId == _currentUser.TenantId)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == _currentUser.TenantId);
        }

        public async Task<List<ProductImage>>CreateRangeAsync(List<ProductImage> images)
        {
            foreach (var image in images)
            {
                image.TenantId = _currentUser.TenantId.Value;
            }
           

          
            await _context.ProductImages
                .AddRangeAsync(images);

            await _context.SaveChangesAsync();

            return images;
        }

        public async Task UpdateAsync(ProductImage image)
        {
            var existingImage =
                await _context.ProductImages
                .FirstOrDefaultAsync(x =>
                    x.Id == image.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingImage == null)
                throw new Exception("Image not found");


            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductImage image)
        {
            var existingImage =
                await _context.ProductImages
                .FirstOrDefaultAsync(x =>
                    x.Id == image.Id &&
                    x.TenantId == _currentUser.TenantId);

            if (existingImage == null)
                throw new Exception("Image not found");

            _context.ProductImages.Remove(existingImage);

            await _context.SaveChangesAsync();
        }
    }
}