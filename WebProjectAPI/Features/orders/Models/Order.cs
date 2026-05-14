namespace WebProjectAPI.Features.orders.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string OrderNumber { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; } // cod, razorpay
        public string PaymentStatus { get; set; }  // pending, paid, failed
        public string OrderStatus { get; set; }    // pending, shipped, delivered

        


        public string? RazorpayOrderId { get; set; }

        public string? RazorpayPaymentId { get; set; }

        public string? RazorpaySignature { get; set; }





        // Billing Info
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
