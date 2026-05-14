using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Interfaces;

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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string search = "")
        {
            var products = await _service.GetAllAsync(pageNumber, pageSize, search);

            return Ok(products);
        }

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
                    message = "Product not found"
                });
            }

            return Ok(product);
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

            return Ok(products);
        }


        [HttpPost("filter")]
        public async Task<IActionResult> FilterProducts(
    ProductFilterDto dto)
        {
            var query = _context.Products
                .AsQueryable();

            // SEARCH

            if (!string.IsNullOrEmpty(dto.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(dto.Search));
            }

            // CATEGORY FILTER

            if (dto.CategoryIds.Any())
            {
                query = query.Where(x =>
                    dto.CategoryIds
                        .Contains(x.CategoryId));
            }

            // BRAND FILTER

            if (dto.BrandIds.Any())
            {
                query = query.Where(x =>
                    dto.BrandIds
                        .Contains(x.BrandId));
            }

            // PRICE FILTER

            query = query.Where(x =>

                (x.DiscountPrice ?? x.Price)
                    >= dto.MinPrice

                &&

                (x.DiscountPrice ?? x.Price)
                    <= dto.MaxPrice
            );

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

                case "latest":

                    query = query.OrderByDescending(x =>
                        x.Id);

                    break;
            }

            // TOTAL COUNT

            var totalCount =
                await query.CountAsync();

            // PAGINATION

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

                pageNumber =
                    dto.PageNumber,

                pageSize =
                    dto.PageSize,

                totalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)dto.PageSize
                    )
            });
        }
    }
}