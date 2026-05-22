using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Features.sub_categories.Models;
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
            var query = _context.Products
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .Include(x => x.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            int totalRecords = await query.CountAsync();

            var products = await query
                .OrderByDescending(x => x.Id)
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





        public IQueryable<Product> GetQueryable()
        {
            return _context.Products;
        }

        public IQueryable<Product> GetLatestProductsAsync()
        {
            return _context.Products;
        }
        public async Task<Product?> GetBySlugAsync(string slug)
        {
            return await _context.Products

                .Include(x => x.Brand)

                .Include(x => x.Category)

                .Include(x => x.SubCategory)

                .Include(x => x.ProductImages)

                .FirstOrDefaultAsync(x =>
                    x.Slug == slug);
        }

        public async Task<List<Product>>
    GetRelatedProductsAsync(
        int categoryId,
        int productId)
        {
            return await _context.Products

                .Include(x => x.ProductImages)

                .Where(x =>

                    x.CategoryId == categoryId &&

                    x.Id != productId &&

                    x.Status == true
                )

                .Take(4)

                .ToListAsync();
        }
        public async Task<List<CategoryWithProductsDto>>
        GetHomeCategoryProductsAsync()
        {
            return await _context.Categories

            .Select(c => new CategoryWithProductsDto
            {
                Id = c.Id,

                Name = c.Name,

                SubCategories = c.SubCategories

                .Select(sc => new SubCategoryWithProductsDto
                {
                   Id = sc.Id,

                    Name = sc.Name,

                    Products = sc.Products

                    .Where(p => p.Status)

                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        DiscountPrice = p.DiscountPrice,
                        Image = p.Image,
                        Slug = p.Slug
                    })

                    .Take(4)

                    .ToList()

                })

                .Where(x => x.Products.Any())

                .ToList()

            })

            .Where(x => x.SubCategories.Any())

            .ToListAsync();
        }
    }
}