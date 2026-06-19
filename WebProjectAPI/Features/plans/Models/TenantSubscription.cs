using WebProjectAPI.Models;

namespace WebProjectAPI.Features.plans.Models
{
    public class TenantSubscription
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public string SubscriptionStatus  { get; set; }= string.Empty;


        public Tenant Tenant { get; set; } = null!;

        public Plan Plan { get; set; } = null!;
    }
}
