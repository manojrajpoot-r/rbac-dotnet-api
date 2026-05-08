using AutoMapper;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;



namespace WebProjectAPI.Features.products.Mappings
{
    public class ProductProfile : Profile
    {
       
            public ProductProfile()
            {
                CreateMap<Product, ProductDto>()

                    .ForMember(dest => dest.CategoryName,
                        opt => opt.MapFrom(src => src.Category.Name))

                    .ForMember(dest => dest.SubCategoryName,
                        opt => opt.MapFrom(src => src.SubCategory.Name))

                    .ForMember(dest => dest.BrandName,
                        opt => opt.MapFrom(src => src.Brand.Name));

                CreateMap<UpdateProductDto, Product>()
                    .ForMember(dest => dest.Image,
                        opt => opt.Ignore());

                CreateMap<CreateProductDto, Product>();

                
            }
        
    }
}