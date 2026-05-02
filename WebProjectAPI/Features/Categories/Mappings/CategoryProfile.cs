using AutoMapper;
using WebProjectAPI.Features.Categories.DTOs;
using WebProjectAPI.Features.Categories.Models;

namespace WebProjectAPI.Features.Categories.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<CreateCategoryDto, Category>();

            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}