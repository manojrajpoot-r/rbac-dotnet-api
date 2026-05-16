using AutoMapper;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WebProjectAPI.Features.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllAsync(
         int pageNumber,
         int pageSize,
         string search)
        {
            var result = await _repository.GetAllAsync(
                pageNumber,
                pageSize,
                search
            );

            var data =
                _mapper.Map<List<CategoryDto>>(result.Data);

            return new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = result.TotalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            category.Slug = SlugHelper.GenerateSlug(dto.Name);
            category.Status = true;

            var result = await _repository.CreateAsync(category);

            return _mapper.Map<CategoryDto>(result);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            _mapper.Map(dto, category);
            category.Slug = SlugHelper.GenerateSlug(dto.Name);
            var updatedCategory = await _repository.UpdateAsync(category);

            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return false;

            return await _repository.DeleteAsync(category);
        }


        public async Task<bool> ChangeStatusAsync(int id)
        {
            return await _repository.ChangeStatusAsync(id);
        }





        public async Task<ApiResponse<List<CategoryDto>>>
         GetCategoriesAsync()
        {
            var categories =
                await _repository.GetCategoriesAsync();

            var data =
                _mapper.Map<List<CategoryDto>>(
                    categories
                );

            return new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Data = data
            };
        }

    }
}