namespace WebProjectAPI.Features.booking.Mappings
{
    using AutoMapper;
    using WebProjectAPI.Features.booking.DTOs;
    using WebProjectAPI.Features.booking.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBookingDto, Booking>();

            CreateMap<BookingServiceItemDto, BookingServiceItem>();

            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.Services,
                    opt => opt.MapFrom(src => src.BookingServiceItems));
        }
    }
}
