namespace WebProjectAPI.Features.payments.DTOs
{
    public class VerifyPaymentDto
    {
        public string TransactionId { get; set; } = string.Empty;
        public bool Success { get; set; }
     
    }
}
