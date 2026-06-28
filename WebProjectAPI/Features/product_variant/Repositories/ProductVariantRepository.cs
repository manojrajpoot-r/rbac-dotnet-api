using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_variant.DTOs;
using WebProjectAPI.Features.product_variant.Interfaces;
using WebProjectAPI.Features.product_variant.Models;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Features.product_variant.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ProductVariantRepository(
            AppDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // LIST
        public async Task<ApiResponse<List<ProductVariantDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.ProductVariants
                       .Include(x => x.Product)
                       .Include(x => x.Color)
                       .Include(x => x.Size)
                       .Where(x => !x.IsDeleted)
                       .AsQueryable();

                            if (!_currentUser.IsPlatformUser)
                            {
                                query = query.Where(x =>
                                    x.TenantId == _currentUser.TenantId);
                            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVariantDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ColorId = x.ColorId,
                    SizeId = x.SizeId,
                    SKU = x.SKU,
                    Price = x.Price,
                    Stock = x.Stock,
                    ProductName = x.Product.Name,
                    ColorName = x.Color.Name,
                    SizeName = x.Size.Name
                })
                .ToListAsync();

            return new ApiResponse<List<ProductVariantDto>>
            {
                Success = true,
                Message = "Variant list fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        // GET BY ID
        public async Task<ApiResponse<ProductVariantDto>> GetById(int id)
        {
            var data = await _context.ProductVariants
                        .Where(x =>
                            x.Id == id &&
                            !x.IsDeleted &&
                          x.TenantId == _currentUser.TenantId)
                .Select(x => new ProductVariantDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ColorId = x.ColorId,
                    SizeId = x.SizeId,
                    SKU = x.SKU,
                    Price = x.Price,
                    Stock = x.Stock
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<ProductVariantDto>
            {
                Success = true,
                Message = "Variant fetched successfully",
                Data = data
            };
        }

        // ADD
        public async Task<ApiResponse<ProductVariantCreateUpdateDto>> Add(ProductVariantCreateUpdateDto model)
        {
            bool exists = await _context.ProductVariants
                    .AnyAsync(x =>
                        x.ProductId == model.ProductId &&
                        x.ColorId == model.ColorId &&
                        x.SizeId == model.SizeId &&
                        !x.IsDeleted &&
                        x.TenantId == _currentUser.TenantId);

            if (exists)
            {
                return new ApiResponse<ProductVariantCreateUpdateDto>
                {
                    Success = false,
                    Message = "Variant already exists"
                };
            }
            var product = await _context.Products
                        .FirstOrDefaultAsync(x =>
                            x.Id == model.ProductId &&
                            x.TenantId == _currentUser.TenantId);

                                if (product == null)
                                {
                                    return new ApiResponse<ProductVariantCreateUpdateDto>
                                    {
                                        Success = false,
                                        Message = "Product not found"
                                    };
                                }
            var variant = new ProductVariant
            {
                TenantId = _currentUser.TenantId.Value,
                ProductId = model.ProductId,
                ColorId = model.ColorId,
                SizeId = model.SizeId,
                SKU = model.SKU,
                Price = model.Price,
                Stock = model.Stock,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _context.ProductVariants.Add(variant);

            await _context.SaveChangesAsync();

            model.Id = variant.Id;

            return new ApiResponse<ProductVariantCreateUpdateDto>
            {
                Success = true,
                Message = "Variant added successfully",
                Data = model
            };
        }

        // UPDATE
        public async Task<ApiResponse<ProductVariantCreateUpdateDto>> Update(int id, ProductVariantCreateUpdateDto model)
        {
            var variant = await _context.ProductVariants
                     .FirstOrDefaultAsync(x =>
                         x.Id == id &&
                         !x.IsDeleted &&
                         x.TenantId == _currentUser.TenantId);

            if (variant == null)
            {
                return new ApiResponse<ProductVariantCreateUpdateDto>
                {
                    Success = false,
                    Message = "Variant not found"
                };
            }

            var product = await _context.Products
    .FirstOrDefaultAsync(x =>
        x.Id == model.ProductId &&
        x.TenantId == _currentUser.TenantId);

            if (product == null)
            {
                return new ApiResponse<ProductVariantCreateUpdateDto>
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            variant.ProductId = model.ProductId;
            variant.ColorId = model.ColorId;
            variant.SizeId = model.SizeId;
            variant.Price = model.Price;
            variant.Stock = model.Stock;
            variant.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ApiResponse<ProductVariantCreateUpdateDto>
            {
                Success = true,
                Message = "Variant updated successfully",
                Data = model
            };
        }

        // DELETE
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var variant = await _context.ProductVariants
                     .FirstOrDefaultAsync(x =>
                         x.Id == id &&
                         x.TenantId == _currentUser.TenantId);

            if (variant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Variant not found"
                };
            }

            variant.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Variant deleted successfully"
            };
        }

        // STATUS CHANGE
        public async Task<ApiResponse<string>> ChangeStatus(int id)
        {
            var variant = await _context.ProductVariants
                        .FirstOrDefaultAsync(x =>
                            x.Id == id &&
                            x.TenantId == _currentUser.TenantId);

            if (variant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Variant not found"
                };
            }

            variant.IsActive = !variant.IsActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Status updated successfully"
            };
        }

        // FRONTEND PRODUCT VARIANTS
        public async Task<List<ProductVariantDto>> GetByProductId(int productId)
        {
            return await _context.ProductVariants
                      .Include(x => x.Color)
                      .Include(x => x.Size)
                      .Where(x =>
                          x.ProductId == productId &&
                          x.IsActive &&
                          !x.IsDeleted &&
                          x.TenantId == _currentUser.TenantId)
                  .Select(x => new ProductVariantDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ColorId = x.ColorId,
                    SizeId = x.SizeId,
                    SKU = x.SKU,
                    Price = x.Price,
                    Stock = x.Stock,
                    ColorName = x.Color.Name,
                    SizeName = x.Size.Name
                })
                .ToListAsync();
        }
    }
}
