using AutoMapper;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Interfaces;
using WebProjectAPI.Features.product_images.Models;
using WebProjectAPI.Features.products.Interfaces;

namespace WebProjectAPI.Features.product_images.Services;

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public ProductImageService(
        IProductImageRepository repository,
        IProductRepository productRepository,
        IImageService imageService,
        IMapper mapper)
    {
        _repository = repository;
        _productRepository = productRepository;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductImageDto>>> GetAllAsync(
        int productId)
    {
        var (images, total) = await _repository.GetAllAsync(productId);

        return new ApiResponse<List<ProductImageDto>>
        {
            Success = true,
            Message = "Success",
            Data = _mapper.Map<List<ProductImageDto>>(images),
            TotalRecords = total
        };
    }

    public async Task<ApiResponse<List<ProductImageDto>>> CreateAsync(
        int productId,
        List<IFormFile> images)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
        {
            return new ApiResponse<List<ProductImageDto>>
            {
                Success = false,
                Message = "Product not found"
            };
        }

        var uploaded = await _imageService.UploadMultipleImagesAsync(
            images,
            "uploads/products");

        var (existing, _) = await _repository.GetAllAsync(productId);

        bool hasMain = existing.Any(x => x.IsMain);

        var list = uploaded
            .Select((x, index) => new ProductImage
            {
                ProductId = productId,
                ImageUrl = x,
                IsMain = !hasMain && index == 0
            })
            .ToList();

        var result = await _repository.CreateRangeAsync(list);

        return new ApiResponse<List<ProductImageDto>>
        {
            Success = true,
            Message = "Uploaded Successfully",
            Data = _mapper.Map<List<ProductImageDto>>(result)
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
            Message = "Deleted Successfully"
        };
    }

    public async Task<ApiResponse<string>> SetPrimaryAsync(int id)
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

        await _repository.SetPrimaryAsync(image);

        return new ApiResponse<string>
        {
            Success = true,
            Message = "Primary Image Updated"
        };
    }
}