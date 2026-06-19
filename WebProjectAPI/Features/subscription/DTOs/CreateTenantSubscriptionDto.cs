namespace WebProjectAPI.Features.subscription.DTOs
{
    public class CreateTenantSubscriptionDto
    {
        public int TenantId { get; set; }
        public int PlanId { get; set; }
    }
}
