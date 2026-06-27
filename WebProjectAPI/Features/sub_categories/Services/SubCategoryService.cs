using AutoMapper;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.sub_categories.DTOs;
using WebProjectAPI.Features.sub_categories.Interfaces;
using WebProjectAPI.Features.sub_categories.Mappings;
using WebProjectAPI.Features.sub_categories .Models;
namespace WebProjectAPI.Features.sub_categories.Services
{
    public class SubCategoryService: ISubCategoryService
    {
        private readonly ISubCategoryRepository _repository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public SubCategoryService(ISubCategoryRepository repository,IImageService 
            imageService,IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }



        public async Task<ApiResponse<List<SubCategoryListDto>>> GetAllAsync(
        PaginationRequest request)
        {
            return await _repository.GetAllAsync(request);
        }

        public async Task<SubCategoryListDto> GetByIdAsync(int id)
        {
            var subcategory = await _repository.GetByIdAsync(id);

            if (subcategory == null)
                return null;

            return _mapper.Map<SubCategoryListDto>(subcategory);
        }

        public async Task<SubCategoryListDto> CreateAsync(CreateSubCategoryDto dto)
        {
            var subCategory = _mapper.Map<SubCategory>(dto);

            if (dto.Image != null)
            {
                subCategory.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/subcategories");
            }

            subCategory.Slug = SlugHelper.GenerateSlug(dto.Name);
            subCategory.Status = true;

            var result = await _repository.CreateAsync(subCategory);

            return _mapper.Map<SubCategoryListDto>(result);
        }

        public async Task<SubCategoryListDto> UpdateAsync(int id, UpdateSubCategoryDto dto)
        {
            var subCategory = await _repository.GetByIdAsync(id);

            if (subCategory == null)
                return null;

            // 🔥 Important: Id map mat hone dena
            _mapper.Map(dto, subCategory);

            // Image update
            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(subCategory.Image))
                {
                    _imageService.DeleteImage(subCategory.Image);
                }

                subCategory.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/subcategories");
            }

            // Slug update
            subCategory.Slug = SlugHelper.GenerateSlug(dto.Name);

            var updatedSubCategory = await _repository.UpdateAsync(subCategory);

            return _mapper.Map<SubCategoryListDto>(updatedSubCategory);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var subCategory = await _repository.GetByIdAsync(id);

            if (subCategory == null)
                return false;

            return await _repository.DeleteAsync(subCategory);
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            return await _repository.ChangeStatusAsync(id);
        }

  

        public async Task<ApiResponse<List<SubCategoryListDto>>>
          GetAllSubCategoriesAsync()
        {
            var result = await _repository.GetAllSubCategoriesAsync();

            var data = _mapper.Map<List<SubCategoryListDto>>(result);

            return new ApiResponse<List<SubCategoryListDto>>
            {
                Success = true,
                Data = data
            };
        }
    }
}
