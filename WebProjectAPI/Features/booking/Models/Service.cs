namespace WebProjectAPI.Features.booking.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }

        public decimal Price { get; set; }

        public int DurationMinutes { get; set; }


        public string Description { get; set; }

   
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    
    }
}
