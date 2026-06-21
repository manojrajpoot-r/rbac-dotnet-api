namespace WebProjectAPI.Features.payments.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;

        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public int? TenantSubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = "";
        public string PaymentGateway { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public DateTime PaymentDate { get; set; }
    }
}
