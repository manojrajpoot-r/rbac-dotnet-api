
using global::WebProjectAPI.Features.Common.ApiResponse;
using global::WebProjectAPI.Features.Common.Paginations;
using global::WebProjectAPI.Features.payments.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebProjectAPI.Features.payments.Interfaces;
namespace WebProjectAPI.Features.payments.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class PaymentController : ControllerBase
        {
            private readonly IPaymentService _paymentService;

            public PaymentController(IPaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            // ================= GET ALL =================

            [HttpPost("GetAll")]
            public async Task<ApiResponse<List<PaymentDto>>> GetAll(
                [FromBody] PaginationRequest request)
            {
                return await _paymentService.GetAll(request);
            }

            // ================= GET BY ID =================

            [HttpGet("GetById/{id}")]
            public async Task<ApiResponse<PaymentDto>> GetById(int id)
            {
                return await _paymentService.GetById(id);
            }

            // ================= CREATE PAYMENT =================

            [HttpPost("Create")]
            public async Task<ApiResponse<PaymentDto>> Create(
                [FromBody] CreatePaymentDto model)
            {
                return await _paymentService.Create(model);
            }

            // ================= UPDATE PAYMENT STATUS =================

            [HttpPost("UpdateStatus")]
            public async Task<ApiResponse<string>> UpdateStatus(
                [FromBody] UpdatePaymentStatusDto model)
            {
                return await _paymentService.UpdateStatus(model);
            }

            // ================= DELETE =================

            [HttpDelete("Delete/{id}")]
            public async Task<ApiResponse<string>> Delete(int id)
            {
                return await _paymentService.Delete(id);
            }

            // ================= GET PAYMENT BY TRANSACTION =================

            [HttpGet("GetByTransaction/{transactionId}")]
            public async Task<ApiResponse<PaymentDto>> GetByTransaction(
                string transactionId)
            {
                return await _paymentService.GetByTransaction(transactionId);
            }
        }
    
}
