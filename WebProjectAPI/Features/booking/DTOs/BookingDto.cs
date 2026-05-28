namespace WebProjectAPI.Features.booking.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public List<BookingServiceItemDto> Services { get; set; }
    }
}
