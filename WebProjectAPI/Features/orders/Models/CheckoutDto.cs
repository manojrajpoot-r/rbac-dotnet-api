namespace WebProjectAPI.Features.orders.Models
{
    public class CheckoutDto
    {
        public string PaymentMethod { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
