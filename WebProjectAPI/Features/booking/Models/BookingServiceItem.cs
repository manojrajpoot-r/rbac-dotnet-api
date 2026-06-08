namespace WebProjectAPI.Features.booking.Models
{
    using System.Text.Json.Serialization;
    using WebProjectAPI.Models;

    public class BookingServiceItem : TenantEntity
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int ServiceId { get; set; }

        public decimal Price { get; set; }

        public Booking Booking { get; set; }   
        public Service Service { get; set; }
    }
}
