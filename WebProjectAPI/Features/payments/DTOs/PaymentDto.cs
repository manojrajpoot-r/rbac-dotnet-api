namespace WebProjectAPI.Features.payments.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int TenantSubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = "";
        public string PaymentGateway { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public DateTime PaymentDate { get; set; }
    }
}
