using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.orders.DTOs;
using WebProjectAPI.Features.orders.Models;
using WebProjectAPI.Features.orders.services;
using WebProjectAPI.Features.orders.Services;

namespace WebProjectAPI.Features.orders.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;
        private readonly OrderService _orderService;
        private readonly AppDbContext _context;
        public PaymentController(
            RazorpayService razorpayService,
           OrderService orderService, AppDbContext context)
        {
            _razorpayService = razorpayService;
            _orderService = orderService;
            _context = context;
        }



        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            int userId = 1;

            var orderId = await _orderService.PlaceOrder(userId, dto);

            return Ok(new
            {
                orderId = orderId,
                message = "Order created"
            });
        }


        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] decimal amount)
        {
            var order = _razorpayService.CreateOrder(amount);

            return Ok(order);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDto dto)
        {
            var isValid = _razorpayService.VerifySignature(
                dto.RazorpayOrderId,
                dto.RazorpayPaymentId,
                dto.RazorpaySignature
            );

            if (!isValid)
                return BadRequest(new { message = "Payment failed" });

            var order = await _orderService.GetOrder(dto.OrderId);

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

    
    }
}
