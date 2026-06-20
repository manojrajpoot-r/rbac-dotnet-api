namespace WebProjectAPI.Features.subscription.DTOs
{
   

        public class TenantSubscriptionDto
        {
            public int Id { get; set; }

            public int TenantId { get; set; }
            public string TenantName { get; set; }=string.Empty;

            public int PlanId { get; set; }
            public string PlanName { get; set; }=string.Empty ;

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal Amount { get; set; }
            public string SubscriptionStatus { get; set; }
        }
    
}
