namespace WebProjectAPI.Features.booking.DTOs
{

   



    public class CreateBookingDto
        {
        public int UserId { get; set; }
        public List<int> ServiceIds { get; set; }
        public DateTime BookingDate { get; set; }

        public TimeSpan BookingTime { get; set; }

        public string PaymentMethod { get; set; }

        public string Address { get; set; }

        public string Notes { get; set; }

        public List<BookingServiceItemDto> Services { get; set; }
    }

}
