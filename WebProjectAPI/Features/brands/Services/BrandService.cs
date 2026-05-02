using AutoMapper;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Interfaces;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Helpers;
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

        public async Task<ApiResponse<List<BrandDto>>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string search)
        {
            var result = await _repository.GetAllAsync(
                pageNumber,
                pageSize,
                search);

            var data = _mapper.Map<List<BrandDto>>(result.Brands);

            return new ApiResponse<List<BrandDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = result.TotalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ApiResponse<BrandDto>> GetByIdAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);

            if (brand == null)
            {
                return new ApiResponse<BrandDto>
                {
                    Success = false,
                    Message = "Brand not found"
                };
            }

            var dto = _mapper.Map<BrandDto>(brand); 
            return new ApiResponse<BrandDto>
            {
                Success = true,
                Data = dto
            };
        }

        public async Task<ApiResponse<BrandDto>> CreateAsync(CreateBrandDto dto)
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

            return new ApiResponse<BrandDto>
            {
                Success = true,
                Data = _mapper.Map<BrandDto>(result)
            };
        }

        public async Task<ApiResponse<BrandDto>> UpdateAsync(int id, UpdateBrandDto dto)
        {
            var brand = await _repository.GetByIdAsync(id);

            if (brand == null)
                return new ApiResponse<BrandDto>
                {
                    Success = false,
                    Message = "Brand not found"
                };

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
            var updatedBrand = await _repository.UpdateAsync(brand);

            return _mapper.Map<ApiResponse<BrandDto>>(updatedBrand);
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
    }
}