namespace WebProjectAPI.Features.booking.DTOs
{
    public class CreateServiceDto
    {

        public int Id { get; set; }
        public string ServiceName { get; set; }

        public decimal Price { get; set; }

        public int DurationMinutes { get; set; }
    }
}
