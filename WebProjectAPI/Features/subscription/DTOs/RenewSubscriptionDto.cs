namespace WebProjectAPI.Features.subscription.DTOs
{
    public class RenewSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public DateTime NewEndDate { get; set; }
        public decimal Amount { get; set; }
    }
}
