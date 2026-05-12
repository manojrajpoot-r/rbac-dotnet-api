using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.orders.DTOs;
using WebProjectAPI.Features.orders.Models;

namespace WebProjectAPI.Features.orders.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> PlaceOrder(int userId, CheckoutDto dto)
        {
            var cartItems = await _context.Carts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
                throw new Exception("Cart is empty");

            decimal subtotal = 0;

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == item.ProductId);

                if (product == null) continue;

                subtotal += (product.DiscountPrice ?? product.Price) * item.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                OrderNumber = Guid.NewGuid().ToString("N")[..8],

                Subtotal = subtotal,
                ShippingAmount = 100,
                TotalAmount = subtotal + 100,

                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                OrderStatus = "Pending",

                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,

                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == item.ProductId);

                if (product == null) continue;

                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    Price = product.DiscountPrice ?? product.Price,
                    Quantity = item.Quantity,
                    Total = (product.DiscountPrice ?? product.Price) * item.Quantity
                });
            }

            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task MarkAsPaid(
            Order order,
            string razorpayOrderId,
            string paymentId,
            string signature)
        {
            order.PaymentStatus = "Paid";
            order.OrderStatus = "Confirmed";

            order.RazorpayOrderId = razorpayOrderId;
            order.RazorpayPaymentId = paymentId;
            order.RazorpaySignature = signature;

            await _context.SaveChangesAsync();
        }
    }
}