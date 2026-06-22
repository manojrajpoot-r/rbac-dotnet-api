using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Features.sub_categories.Models;
using WebProjectAPI.Models;
using WebProjectAPI.Services.Implementations;
using WebProjectAPI.Services.Interfaces;


namespace WebProjectAPI.Features.products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public ProductRepository(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUser = currentUserService;
        }

        public async Task<ApiResponse<List<ProductDto>>> GetAllAsync(
            PaginationRequest request)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .Include(x => x.Brand)
                .AsQueryable();

            if (!_currentUser.IsPlatformUser)
            {
                query = query.Where(x =>
                    x.TenantId == _currentUser.TenantId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    Quantity = x.Quantity,
                    Status = x.Status,
                    CategoryName = x.Category.Name,
                    SubCategoryName = x.SubCategory.Name,
                    BrandName = x.Brand.Name
                })
                .ToListAsync();

            return new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
            .FirstOrDefaultAsync(x =>
            x.Id == id && x.TenantId == _currentUser.TenantId);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.TenantId = _currentUser.TenantId.Value;
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
           var existing = await _context.Products
                        .FirstOrDefaultAsync(x =>
                x.Id == product.Id &&
                x.TenantId == _currentUser.TenantId);

            if (existing == null)
            {
                throw new Exception("Product not found");
            }
            _context.Products.Update(existing);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            var existing = await _context.Products
                        .FirstOrDefaultAsync(x =>
                x.Id == product.Id &&
                x.TenantId == _currentUser.TenantId);

            if (existing == null)
            {
                throw new Exception("Product not found");
            }
            _context.Products.Remove(existing);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var product = await _context.Products
             .FirstOrDefaultAsync(x =>
                 x.Id == id &&
                 x.TenantId == _currentUser.TenantId);

            if (product == null)
                return false;

            product.Status = !product.Status;

            await _context.SaveChangesAsync();

            return true;
        }





        public IQueryable<Product> GetQueryable()
        {
            return _context.Products
         .Where(x => x.TenantId == _currentUser.TenantId);
        }

        public IQueryable<Product> GetLatestProductsAsync()
        {
            return _context.Products
        .Where(x => x.TenantId == _currentUser.TenantId);
        }
        public async Task<Product?> GetBySlugAsync(string slug)
        {
            return await _context.Products
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.ProductImages)
            .FirstOrDefaultAsync(x =>
                x.Slug == slug &&
                x.TenantId == _currentUser.TenantId);
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
                x.Status &&
                x.TenantId == _currentUser.TenantId)
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

                    .Where(p =>
                    p.Status &&
                    p.TenantId == _currentUser.TenantId)

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