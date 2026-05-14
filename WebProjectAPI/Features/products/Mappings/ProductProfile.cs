using AutoMapper;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;

using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Models;

namespace WebProjectAPI.Features.products.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductImage, ProductImageDto>();

            CreateMap<Product, ProductDto>()

                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.Category.Name))

                .ForMember(dest => dest.SubCategoryName,
                    opt => opt.MapFrom(src =>
                        src.SubCategory.Name))

                .ForMember(dest => dest.BrandName,
                    opt => opt.MapFrom(src =>
                        src.Brand.Name))

                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.ProductImages));

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Image,
                    opt => opt.Ignore());

            CreateMap<CreateProductDto, Product>();
        }
    }
}