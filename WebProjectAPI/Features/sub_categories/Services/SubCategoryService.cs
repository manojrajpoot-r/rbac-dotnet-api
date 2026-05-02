using AutoMapper;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.products.Models;
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

        public SubCategoryService(
            ISubCategoryRepository repository,
            IImageService imageService,
            IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }


        public async Task<List<SubCategoryDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return _mapper.Map<List<SubCategoryDto>>(data);
        }

        public async Task<SubCategoryDto> GetByIdAsync(int id)
        {
            var subcategory = await _repository.GetByIdAsync(id);

            if (subcategory == null)
                return null;

            return _mapper.Map<SubCategoryDto>(subcategory);
        }

        public async Task<SubCategoryDto> CreateAsync(CreateSubCategoryDto dto)
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

            return _mapper.Map<SubCategoryDto>(result);
        }

        public async Task<SubCategoryDto> UpdateAsync(int id, UpdateSubCategoryDto dto)
        {
            var subCategory = await _repository.GetByIdAsync(id);

            if (subCategory == null)
                return null;

            _mapper.Map(dto, subCategory);

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(subCategory.Image))
                {
                    _imageService.DeleteImage(subCategory.Image);
                }
               
                subCategory.Image = await _imageService
                    .UploadImageAsync(dto.Image, "uploads/subcategories");
            }
            subCategory.Slug = SlugHelper.GenerateSlug(dto.Name);
            var updatedSubCategory = await _repository.UpdateAsync(subCategory);

            return _mapper.Map<SubCategoryDto>(updatedSubCategory);
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
    }
}
