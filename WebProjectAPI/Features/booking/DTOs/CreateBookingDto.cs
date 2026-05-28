namespace WebProjectAPI.Features.booking.DTOs
{

    public class CreateBookingDto
    {
        public int UserId { get; set; }

        public DateTime BookingDate { get; set; }

        public string BookingTime { get; set; }

        public List<int> ServiceIds { get; set; }

        public string PaymentMethod { get; set; }

        public string? Notes { get; set; }

        public string? Address { get; set; }
    }

}
