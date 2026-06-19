namespace WebProjectAPI.Features.payments.DTOs
{
    public class CreatePaymentDto
    {
        public int TenantSubscriptionId { get; set; }
        public string PaymentGateway { get; set; } = "Razorpay";
    }
}
