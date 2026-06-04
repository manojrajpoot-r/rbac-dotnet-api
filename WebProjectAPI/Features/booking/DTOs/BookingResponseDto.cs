namespace WebProjectAPI.Features.booking.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }


        public string UserName { get; set; }

        public string BookingDate { get; set; }
        public string BookingTime { get; set; }

        public decimal TotalAmount { get; set; }

        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; }
        public List<BookingServiceItemDto> Services { get; set; }
    }
}
