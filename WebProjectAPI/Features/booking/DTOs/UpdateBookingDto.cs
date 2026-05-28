namespace WebProjectAPI.Features.booking.DTOs
{
  
        public class UpdateBookingDto
        {
            public int Id { get; set; }

            public int UserId { get; set; }

            public DateTime BookingDate { get; set; }

            public List<int> ServiceIds { get; set; }
        }
    
}
