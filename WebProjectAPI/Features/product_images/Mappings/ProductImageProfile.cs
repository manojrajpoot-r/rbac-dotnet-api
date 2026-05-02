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

            CreateMap<Product, ProductDto>()
           .ForMember(dest => dest.Images,
               opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<ProductImage, ProductImageDto>();
        }

    }
}
