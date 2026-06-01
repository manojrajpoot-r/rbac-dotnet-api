namespace WebProjectAPI.Features.booking.DTOs
{
    public class BookingServiceItemDto
    {
        public int ServiceId { get; set; }
        public decimal Price { get; set; }
        public string ServiceName { get; set; }   // 👈 ADD THIS
    }
}
