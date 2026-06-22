using AutoMapper;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.Common.Helpers;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
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

        public async Task<ApiResponse<List<CategoryListDto>>> GetAllAsync(
        PaginationRequest request)
        {
            var result = await _repository.GetAllAsync( request);

            var data =
                _mapper.Map<List<CategoryListDto>>(result.Data);

            return new ApiResponse<List<CategoryListDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = result.TotalRecords,
            };
        }

        public async Task<CategoryListDto> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return _mapper.Map<CategoryListDto>(category);
        }

        public async Task<CategoryListDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            category.Slug = SlugHelper.GenerateSlug(dto.Name);
            category.Status = true;

            var result = await _repository.CreateAsync(category);

            return _mapper.Map<CategoryListDto>(result);
        }

        public async Task<CategoryListDto> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            _mapper.Map(dto, category);
            category.Slug = SlugHelper.GenerateSlug(dto.Name);
            var updatedCategory = await _repository.UpdateAsync(category);

            return _mapper.Map<CategoryListDto>(updatedCategory);
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





        public async Task<ApiResponse<List<CategoryListDto>>>
         GetCategoriesAsync()
        {
            var categories =
                await _repository.GetCategoriesAsync();

            var data =
                _mapper.Map<List<CategoryListDto>>(
                    categories
                );

            return new ApiResponse<List<CategoryListDto>>
            {
                Success = true,
                Data = data
            };
        }



    }
}