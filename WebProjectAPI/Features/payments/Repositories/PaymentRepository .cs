using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.payments.Constants;
using WebProjectAPI.Features.payments.DTOs;
using WebProjectAPI.Features.payments.Interfaces;
using WebProjectAPI.Features.payments.Models;
namespace WebProjectAPI.Features.payments.Repositories
{
  
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<List<PaymentDto>>> GetAll(PaginationRequest request)
        {
            var query = _context.Payments.AsQueryable();

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.TransactionId.Contains(request.Search) ||
                    x.PaymentGateway.Contains(request.Search) ||
                    x.PaymentStatus.Contains(request.Search));
            }

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaymentDto
                {
                    Id = x.Id,
                    TenantSubscriptionId = x.TenantSubscriptionId,
                    Amount = x.Amount,
                    TransactionId = x.TransactionId,
                    PaymentGateway = x.PaymentGateway,
                    PaymentStatus = x.PaymentStatus,
                    PaymentDate = x.PaymentDate
                })
                .ToListAsync();

            return new ApiResponse<List<PaymentDto>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        public async Task<ApiResponse<PaymentDto>> GetById(int id)
        {
            var data = await _context.Payments
                .Where(x => x.Id == id)
                .Select(x => new PaymentDto
                {
                    Id = x.Id,
                    TenantSubscriptionId = x.TenantSubscriptionId,
                    Amount = x.Amount,
                    TransactionId = x.TransactionId,
                    PaymentGateway = x.PaymentGateway,
                    PaymentStatus = x.PaymentStatus,
                    PaymentDate = x.PaymentDate
                })
                .FirstOrDefaultAsync();

            if (data == null)
            {
                return new ApiResponse<PaymentDto>
                {
                    Success = false,
                    Message = "Payment not found"
                };
            }

            return new ApiResponse<PaymentDto>
            {
                Success = true,
                Message = "Payment fetched successfully",
                Data = data
            };
        }
        // ================= CREATE PAYMENT =================
        public async Task<ApiResponse<PaymentDto>> Create(CreatePaymentDto model)
        {
            var sub = await _context.TenantSubscriptions
                .FirstOrDefaultAsync(x => x.Id == model.TenantSubscriptionId);

            if (sub == null)
            {
                return new ApiResponse<PaymentDto>
                {
                    Success = false,
                    Message = "Subscription not found"
                };
            }

            var payment = new Payment
            {
                TenantSubscriptionId = model.TenantSubscriptionId,
                Amount = sub.Amount,
                PaymentGateway = model.PaymentGateway,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new ApiResponse<PaymentDto>
            {
                Success = true,
                Message = "Payment created successfully",
                Data = new PaymentDto
                {
                    Id = payment.Id,
                    TenantSubscriptionId = payment.TenantSubscriptionId,
                    Amount = payment.Amount,
                    TransactionId = payment.TransactionId,
                    PaymentGateway = payment.PaymentGateway,
                    PaymentStatus = payment.PaymentStatus,
                    PaymentDate = payment.PaymentDate
                }
            };
        }
        // ================= UPDATE STATUS =================
        public async Task<ApiResponse<string>> UpdateStatus(UpdatePaymentStatusDto model)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(x => x.TransactionId == model.TransactionId);

            if (payment == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Payment not found"
                };
            }

            payment.PaymentStatus = model.PaymentStatus;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Payment status updated successfully"
            };
        }
        public async Task<ApiResponse<string>> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Payment not found"
                };
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Payment deleted successfully"
            };
        }
        // ================= GET BY TRANSACTION =================
        public async Task<ApiResponse<PaymentDto>> GetByTransaction(string transactionId)
        {
            var data = await _context.Payments
                .Where(x => x.TransactionId == transactionId)
                .Select(x => new PaymentDto
                {
                    Id = x.Id,
                    TenantSubscriptionId = x.TenantSubscriptionId,
                    Amount = x.Amount,
                    TransactionId = x.TransactionId,
                    PaymentGateway = x.PaymentGateway,
                    PaymentStatus = x.PaymentStatus,
                    PaymentDate = x.PaymentDate
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<PaymentDto>
            {
                Success = data != null,
                Message = data != null ? "Payment found" : "Payment not found",
                Data = data
            };
        }
    }
}

