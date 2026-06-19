namespace WebProjectAPI.Features.plans.DTOs
{
   
        public class PlanDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int DurationInMonths { get; set; }
            public int MaxUsers { get; set; }
            public bool IsActive { get; set; }
        }
    
}
