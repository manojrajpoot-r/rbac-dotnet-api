namespace WebProjectAPI.Features.booking.Models
{
    using System.Text.Json.Serialization;

    public class BookingServiceItem
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        [JsonIgnore]
        public Booking Booking { get; set; }

        public int ServiceId { get; set; }

        [JsonIgnore]
        public Service Service { get; set; }

        public decimal Price { get; set; }
    }
}
