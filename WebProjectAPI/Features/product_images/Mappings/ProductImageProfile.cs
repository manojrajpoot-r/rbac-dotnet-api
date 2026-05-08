using WebProjectAPI.Features.product_images.DTOs;
using WebProjectAPI.Features.product_images.Models;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;
using AutoMapper;
namespace WebProjectAPI.Features.product_images.Mappings
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {

            CreateMap<ProductImage, ProductImageDto>()

            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name));



            CreateMap<Product, ProductDto>()
           .ForMember(dest => dest.Images,
               opt => opt.MapFrom(src => src.ProductImages));

           
        }

    }
}
