namespace WebProjectAPI.Features.payments.DTOs
{
    public class UpdatePaymentStatusDto
    {
        public string TransactionId { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
