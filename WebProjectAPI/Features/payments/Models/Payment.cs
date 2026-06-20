using WebProjectAPI.Features.subscription.Models;

namespace WebProjectAPI.Features.payments.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int? TenantSubscriptionId { get; set; }

        public int TenantId { get; set; }

        public int PlanId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionId { get; set; } = string.Empty;

        public string PaymentGateway { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public TenantSubscription? TenantSubscription { get; set; }
    }
}
