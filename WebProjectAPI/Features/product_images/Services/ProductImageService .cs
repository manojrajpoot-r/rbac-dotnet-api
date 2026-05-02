using AutoMapper;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Interfaces;
using WebProjectAPI.Features.product_images.Models;
using WebProjectAPI.Helpers;

namespace WebProjectAPI.Features.product_images.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductImageService(
            IProductImageRepository repository,
            IImageService imageService,
            IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ProductImageDto>>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string search)
        {
            var (images, totalRecords) =
                await _repository.GetAllAsync(
                    pageNumber,
                    pageSize,
                    search);

            var data = _mapper.Map<List<ProductImageDto>>(images);

            return new ApiResponse<List<ProductImageDto>>
            {
                Success = true,
                Message = "Product images fetched successfully",
                Data = data,
                TotalRecords = totalRecords
            };
        }

        public async Task<ApiResponse<List<ProductImageDto>>> CreateAsync(
            int productId,
            List<IFormFile> images)
        {
            var uploadedImages = await _imageService
                .UploadMultipleImagesAsync(
                    images,
                    "uploads/products");

            var productImages = uploadedImages
                .Select((img, index) => new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = img,
                    IsMain = index == 0
                }).ToList();

            var createdImages =
                await _repository.CreateRangeAsync(productImages);

            return new ApiResponse<List<ProductImageDto>>
            {
                Success = true,
                Message = "Images uploaded successfully",
                Data = _mapper.Map<List<ProductImageDto>>(createdImages)
            };
        }

        public async Task<ApiResponse<ProductImageDto>> UpdateAsync(
            int id,
            ProductImageUpdateDto dto)
        {
            var image = await _repository.GetByIdAsync(id);

            if (image == null)
            {
                return new ApiResponse<ProductImageDto>
                {
                    Success = false,
                    Message = "Image not found"
                };
            }

            if (dto.Image != null)
            {
                var uploadedImage = await _imageService
                    .UploadImageAsync(
                        dto.Image,
                        "uploads/products");

                image.ImageUrl = uploadedImage;
            }

            image.ProductId = dto.ProductId;
            image.IsMain = dto.IsMain;

            await _repository.UpdateAsync(image);

            return new ApiResponse<ProductImageDto>
            {
                Success = true,
                Message = "Image updated successfully",
                Data = _mapper.Map<ProductImageDto>(image)
            };
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var image = await _repository.GetByIdAsync(id);

            if (image == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Image not found"
                };
            }

            await _repository.DeleteAsync(image);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Image deleted successfully"
            };
        }
    }
}