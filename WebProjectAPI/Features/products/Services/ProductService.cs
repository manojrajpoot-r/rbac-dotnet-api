using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Helpers;
namespace WebProjectAPI.Features.products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ProductService(
            IProductRepository repository,
            IMapper mapper,
            IImageService imageService)
        {
            _repository = repository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ApiResponse<List<ProductDto>>> GetAllAsync(
            int pageNumber,
            int pageSize,
             string search)
        {
            var result = await _repository.GetAllAsync(
                pageNumber,
                pageSize,
                search);

            var data = _mapper.Map<List<ProductDto>>(result.Products);

            return new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = result.TotalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            var dto = _mapper.Map<ProductDto>(product);

            return new ApiResponse<ProductDto>
            {
                Success = true,
                Data = dto
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            if (dto.Image != null)
            {
                product.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/products");
            }
            product.Slug = SlugHelper.GenerateSlug(dto.Name);
            product.SKU = SKUHelper.GenerateSKU("PRD");
            product.Status = true;

            var result = await _repository.CreateAsync(product);

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            _mapper.Map(dto, product);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    _imageService.DeleteImage(product.Image);
                }

                product.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/products");
            }
            product.Slug = SlugHelper.GenerateSlug(dto.Name);
          
            var updatedProduct = await _repository.UpdateAsync(product);

            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            if (!string.IsNullOrEmpty(product.Image))
            {
                _imageService.DeleteImage(product.Image);
            }

            return await _repository.DeleteAsync(product);
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            return await _repository.ChangeStatusAsync(id);
        }













  


        //frontend

        public async Task<ApiResponse<List<ProductDto>>> GetFeaturedProductsAsync()
        {
            var products = await _repository
                    .GetQueryable()
                    .Include(x => x.Category)
                    .Include(x => x.Brand)

                .Where(x => x.IsFeatured)
            .Select(x => new ProductDto
            {
                Id = x.Id,

                Name = x.Name,

                Price = x.Price,

                DiscountPrice = x.DiscountPrice,

                DiscountPercentage = x.DiscountPercentage,

                Image = x.Image,

                Status = x.Status,

                Slug = x.Slug,

                CategoryId = x.CategoryId,

                CategoryName = x.Category.Name,

                BrandId = x.BrandId,

                BrandName = x.Brand.Name
            })
                .ToListAsync();
           
            return new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = products
            };
        }

        public async Task<ApiResponse<List<ProductDto>>> GetLatestProductsAsync()
        {
            var products = await _repository
                .GetQueryable()
                .OrderByDescending(x => x.CreatedAt)
                .Take(8)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    DiscountPercentage = x.DiscountPercentage,
                    Image = x.Image,
                    Slug = x.Slug
                })
                .ToListAsync();
           
            return new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = products
            };
        }


        public async Task<ProductDto?> GetBySlugAsync(string slug)
        {
            var product =
                await _repository.GetBySlugAsync(slug);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>>
       GetRelatedProductsAsync(
           int categoryId,
           int productId)
        {
            var products =
                await _repository
                .GetRelatedProductsAsync(
                    categoryId,
                    productId
                );

            return _mapper.Map<List<ProductDto>>(
                products
            );
        }
    }
}