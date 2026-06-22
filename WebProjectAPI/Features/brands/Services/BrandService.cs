using AutoMapper;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Interfaces;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Helpers;
using WebProjectAPI.Features.Common.ApiResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WebProjectAPI.Features.brands.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public BrandService(
            IBrandRepository repository,
            IMapper mapper,
            IImageService imageService)
        {
            _repository = repository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ApiResponse<List<BrandListDto>>> GetAllAsync(
           PaginationRequest request)
        {
            return await _repository.GetAllAsync(request);


         
        }

        public async Task<ApiResponse<BrandListDto>> GetByIdAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);

            if (brand == null)
            {
                return new ApiResponse<BrandListDto>
                {
                    Success = false,
                    Message = "Brand not found"
                };
            }

            var dto = _mapper.Map<BrandListDto>(brand); 
            return new ApiResponse<BrandListDto>
            {
                Success = true,
                Data = dto
            };
        }

        public async Task<ApiResponse<BrandListDto>> CreateAsync(CreateBrandDto dto)
        {
            var brand = _mapper.Map<Brand>(dto);

            if (dto.Image != null)
            {
                brand.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/brands");
            }

            brand.Slug = SlugHelper.GenerateSlug(dto.Name);
            brand.Status = true;

            var result = await _repository.CreateAsync(brand);

            return new ApiResponse<BrandListDto>
            {
                Success = true,
                Data = _mapper.Map<BrandListDto>(result)
            };
        }

        public async Task<ApiResponse<BrandListDto>> UpdateAsync(
            int id,
            UpdateBrandDto dto)
        {
            var brand = await _repository.GetByIdAsync(id);

            if (brand == null)
            {
                return new ApiResponse<BrandListDto>
                {
                    Success = false,
                    Message = "Brand not found"
                };
            }

            _mapper.Map(dto, brand);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(brand.Image))
                {
                    _imageService.DeleteImage(brand.Image);
                }

                brand.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/brands");
            }

            brand.Slug = SlugHelper.GenerateSlug(dto.Name);

            var updatedBrand =
                await _repository.UpdateAsync(brand);

            return new ApiResponse<BrandListDto>
            {
                Success = true,
                Message = "Brand updated successfully",
                Data = _mapper.Map<BrandListDto>(updatedBrand)
            };
        }
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);

            if (brand == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Brand not found"
                };

            if (!string.IsNullOrEmpty(brand.Image))
            {
                _imageService.DeleteImage(brand.Image);
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = await _repository.DeleteAsync(brand)
            };
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(int id)
        {
            var result = await _repository.ChangeStatusAsync(id);

            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Brand not found"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = result
            };
        }




        public async Task<ApiResponse<List<BrandListDto>>>
     GetAllBrandsAsync()
        {
            var result =
                await _repository.GetAllBrandsAsync();

            var data =
                _mapper.Map<List<BrandListDto>>(result);

            return new ApiResponse<List<BrandListDto>>
            {
                Success = true,
                Data = data
            };
        }
    }
}