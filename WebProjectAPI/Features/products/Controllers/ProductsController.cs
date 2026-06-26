using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Services;
using WebProjectAPI.Attributes;
namespace WebProjectAPI.Features.products.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly AppDbContext _context;

        public ProductsController(IProductService service,AppDbContext context)
        {
            _service = service;
            _context=context;
        }

        [Authorize]
        [Permission("PRODUCT_VIEW")]
        
        [HttpPost("list")]
        public async Task<IActionResult> GetAll(PaginationRequest request)
        {
            var products = await _service.GetAllAsync(request);

            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(product);
        }

        [Authorize]
        [Permission("PRODUCT_CREATE")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            var product = await _service.CreateAsync(dto);

            return Ok(new
            {
                message = "Product created successfully",
                data = product
            });
        }
        [Authorize]
        [Permission("PRODUCT_EDIT")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto dto)
        {
            var product = await _service.UpdateAsync(id, dto);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                message = "Product updated successfully",
                data = product
            });
        }
        [Authorize]
        [Permission("PRODUCT_DELETE")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                message = "Product deleted successfully"
            });
        }


 










        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            var products = await _service.GetFeaturedProductsAsync();

            return Ok(products);
        }
      
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProductsAsync()
        {
            var products = await _service.GetLatestProductsAsync();

            return Ok(products);
        }

    
        [HttpGet("slug/{slug}")]
        
        public async Task<IActionResult> GetProduct(string slug)
        {
            var product =
                await _service.GetBySlugAsync(slug);

            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                success = true,
                data = product
            });
        }

        [HttpGet("related/{categoryId}/{productId}")]
        public async Task<IActionResult>
            RelatedProducts(
          int categoryId,
          int productId)
        {
            var products =
                await _service
                .GetRelatedProductsAsync(
                    categoryId,
                    productId
                );
            return Ok(new
            {
                success = true,
                data = products
            });
          
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts(
     [FromBody] ProductFilterDto dto)
        {
            var query = _context.Products
                    .Where(x => x.Status)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(dto.Search));
            }

            if (dto.CategoryIds?.Any() == true)
            {
                query = query.Where(x =>
                    dto.CategoryIds.Contains(x.CategoryId));
            }

            if (dto.BrandIds?.Any() == true)
            {
                query = query.Where(x =>
                    dto.BrandIds.Contains(x.BrandId));
            }

            if (dto.MinPrice > 0 || dto.MaxPrice > 0)
            {
                query = query.Where(x =>

                    ((x.DiscountPrice) ?? 0)
                    >= dto.MinPrice

                    &&

                    ((x.DiscountPrice) ?? 0)
                    <= dto.MaxPrice
                );
            }

            // SORTING

            switch (dto.SortBy)
            {
                case "lowToHigh":

                    query = query.OrderBy(x =>
                        x.DiscountPrice ?? x.Price);

                    break;

                case "highToLow":

                    query = query.OrderByDescending(x =>
                        x.DiscountPrice ?? x.Price);

                    break;

                default:

                    query = query.OrderByDescending(x =>
                        x.Id);

                    break;
            }

            // TOTAL

            var totalCount =
                await query.CountAsync();

            // DATA

            var products = await query

                .Skip(
                    (dto.PageNumber - 1)
                    * dto.PageSize
                )

                .Take(dto.PageSize)

                .ToListAsync();

            return Ok(new
            {
                data = products,

                totalCount,

                pageNumber = dto.PageNumber,

                pageSize = dto.PageSize,

                totalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)dto.PageSize
                    )
            });
        }

        [HttpGet("home-category-products")]
        public async Task<IActionResult>
            GetHomeCategoryProducts()
                    {
                        var result = await _service
                            .GetHomeCategoryProductsAsync();

                        return Ok(result);
                    }
    }
}