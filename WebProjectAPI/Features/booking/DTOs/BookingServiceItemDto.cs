namespace WebProjectAPI.Features.booking.DTOs
{
    public class BookingServiceItemDto
    {
        public int ServiceId { get; set; }
        public decimal Price { get; set; }
        public string ServiceName { get; set; }
         
        public string Description { get; set; }

           
         public int DurationMinutes { get; set; }

         public string ImageUrl { get; set; }

         public bool IsActive { get; set; }
        
    }
}
