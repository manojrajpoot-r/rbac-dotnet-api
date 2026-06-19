namespace WebProjectAPI.Features.plans.DTOs
{
    
         public class CreateplanDto
        {
            public string Name { get; set; } = string.Empty;

            public decimal Price { get; set; }

            public int DurationInMonths { get; set; }

            public int MaxUsers { get; set; }
        }
    
}
