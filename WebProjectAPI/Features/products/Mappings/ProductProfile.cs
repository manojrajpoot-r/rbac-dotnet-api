using AutoMapper;
using WebProjectAPI.Features.products.DTOs;
using WebProjectAPI.Features.products.Models;



namespace WebProjectAPI.Features.products.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>();

            CreateMap<UpdateProductDto, Product>();
        }
    }
}