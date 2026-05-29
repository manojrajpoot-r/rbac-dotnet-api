using WebProjectAPI.Models;

namespace WebProjectAPI.Features.booking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime BookingDate { get; set; }

        // TIME SLOT
        public string BookingTime { get; set; }

        // TOTAL
        public decimal TotalAmount { get; set; }

        // BOOKING STATUS
        public string BookingStatus { get; set; }
        // Pending / Confirmed / Completed / Cancelled

        // PAYMENT
        public string PaymentStatus { get; set; }
        // Pending / Paid

        public string PaymentMethod { get; set; }
        // Cash / UPI / Card

        // EXTRA
        public string? Notes { get; set; }

        public string? Address { get; set; }

        // ACTIVE
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public List<BookingServiceItem> BookingServiceItems { get; set; }
    }
}
