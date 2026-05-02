using AutoMapper;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.brands.DTOs;
using WebProjectAPI.Features.brands.Models;

namespace WebProjectAPI.Features.brands.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDto>();

            CreateMap<CreateBrandDto, Brand>();

            CreateMap<UpdateBrandDto, Brand>();
        }
    }
}