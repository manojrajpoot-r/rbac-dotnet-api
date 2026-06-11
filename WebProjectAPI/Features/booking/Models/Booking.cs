using WebProjectAPI.Models;

namespace WebProjectAPI.Features.booking.Models
{
    public class Booking : TenantEntity
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public DateTime BookingDate { get; set; }

        // TIME SLOT
        public string BookingTime { get; set; } = string.Empty;

        // TOTAL
        public decimal TotalAmount { get; set; }

        // STATUS
        public string BookingStatus { get; set; } = "Pending";

        public string PaymentStatus { get; set; } = "Pending";

        public string PaymentMethod { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public string? Address { get; set; }

        // NAVIGATION
        public List<BookingServiceItem> BookingServiceItems { get; set; }
            = new List<BookingServiceItem>();
    }
}