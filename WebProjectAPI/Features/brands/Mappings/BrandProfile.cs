using AutoMapper;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Models;


namespace WebProjectAPI.Features.brands.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandListDto>();

            CreateMap<CreateBrandDto, Brand>();

            CreateMap<UpdateBrandDto, Brand>();
        }
    }
}