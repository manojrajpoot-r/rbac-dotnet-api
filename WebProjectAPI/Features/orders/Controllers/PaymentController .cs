using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebProjectAPI.Data;
using WebProjectAPI.Features.orders.DTOs;
using WebProjectAPI.Features.orders.Models;
using WebProjectAPI.Features.orders.services;
using WebProjectAPI.Features.orders.Services;
using WebProjectAPI.Models;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Features.orders.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;
        private readonly OrderService _orderService;
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public PaymentController(
            RazorpayService razorpayService,
           OrderService orderService, AppDbContext context, ICurrentUserService currentUserService)
        {
            _razorpayService = razorpayService;
            _orderService = orderService;
            _context = context;
            _currentUser = currentUserService;
        }



        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim);

            var orderId = await _orderService.PlaceOrder(userId, dto);

            return Ok(new
            {
                orderId = orderId,
                message = "Order created"
            });
        }


        [HttpPost("create-order")]
        public IActionResult CreateOrder(
      [FromBody] decimal amount)
        {
            var order =
                _razorpayService.CreateOrder(amount);

            return Ok(new
            {
                key = _razorpayService.GetKey(),

                orderId = order["id"].ToString(),

                amount =  order["amount"],

                currency = order["currency"],

                   
        });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] OrderVerifyPaymentDto dto)
        {
            var isValid = _razorpayService.VerifySignature(
                dto.RazorpayOrderId,
                dto.RazorpayPaymentId,
                dto.RazorpaySignature
            );

            if (!isValid)
                return BadRequest(new { message = "Payment failed" });

            var order = await _context.Orders
                         .FirstOrDefaultAsync(x =>
                             x.Id == dto.OrderId &&
                             x.TenantId == _currentUser.TenantId);

            if (order == null)
            {
                return NotFound();
            }

            await _orderService.MarkAsPaid(
                order,
                dto.RazorpayOrderId,
                dto.RazorpayPaymentId,
                dto.RazorpaySignature
            );

            return Ok(new
            {
                message = "Payment success"
            });
        }

        [HttpPost("payment-failed")]
        public async Task<IActionResult> PaymentFailed(
       [FromBody] PaymentFailedDto dto)
        {
            var order = await _context.Orders
      .FirstOrDefaultAsync(x =>
          x.Id == dto.OrderId &&
          x.TenantId == _currentUser.TenantId);

            if (order == null)
            {
                return NotFound();
            }

            order.PaymentStatus = "Failed";

            order.OrderStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment failed status updated"
            });
        }

    }
}
