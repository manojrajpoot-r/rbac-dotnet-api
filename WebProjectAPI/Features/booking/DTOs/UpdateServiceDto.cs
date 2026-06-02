namespace WebProjectAPI.Features.booking.DTOs
{
    public class UpdateServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }

        public decimal Price { get; set; }

        public int DurationMinutes { get; set; }
 
        public string Description { get; set; } = string.Empty;
     
        public IFormFile? ImageUrl { get; set; }
        public DateTime Updated { get; set; }
    }
}
