using AutoMapper;
using WebProjectAPI.Features.sub_categories.DTOs;
using WebProjectAPI.Features.sub_categories.Models;
namespace WebProjectAPI.Features.sub_categories.Mappings
{
    public class SubCategoryProfile:Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryDto>()
             .ForMember(
                 dest => dest.CategoryName,
                 opt => opt.MapFrom(src => src.Category.Name)
             );

            CreateMap<CreateSubCategoryDto, SubCategory>();

            CreateMap<UpdateSubCategoryDto, SubCategory>();
        }
    }
}
