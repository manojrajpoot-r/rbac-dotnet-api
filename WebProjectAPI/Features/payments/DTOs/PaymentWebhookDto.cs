namespace WebProjectAPI.Features.payments.DTOs
{
    public class PaymentWebhookDto
    {
        public string TransactionId { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
