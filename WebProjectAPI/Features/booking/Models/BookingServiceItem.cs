namespace WebProjectAPI.Features.booking.Models
{
    public class BookingServiceItem
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public Booking Booking { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public decimal Price { get; set; }
    }
}
