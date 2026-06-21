using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using System.Numerics;
using WebProjectAPI.Data;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.payments.Constants;
using WebProjectAPI.Features.payments.DTOs;
using WebProjectAPI.Features.payments.Interfaces;
using WebProjectAPI.Features.payments.Models;
using WebProjectAPI.Features.plans.Models;
using WebProjectAPI.Features.subscription.Models;
using WebProjectAPI.Models;
using WebProjectAPI.Services.Interfaces;
using Payment = WebProjectAPI.Features.payments.Models.Payment;
namespace WebProjectAPI.Features.payments.Repositories
{
  
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly RazorpaySettings _razorpay;
        private readonly ICurrentUserService _currentUser;
        public PaymentRepository(AppDbContext context, IOptions<RazorpaySettings> razorpayOptions, ICurrentUserService currentUser)
        {
            _context = context;
            _razorpay = razorpayOptions.Value;
            _currentUser = currentUser;
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
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    PlanId = x.PlanId,
                    PlanName = x.Plan.Name,
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


        public async Task<ApiResponse<object>> CreateOrder(CreateOrderDto model)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(x => x.Id == model.PlanId);

            if (plan == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Subscription not found"
                };
            }

            RazorpayClient client =
                new RazorpayClient(
                    _razorpay.KeyId,
                    _razorpay.KeySecret);

            Dictionary<string, object> options = new();

            options.Add("amount", (int)(plan.Price * 100));
            options.Add("currency", "INR");
            options.Add("receipt", Guid.NewGuid().ToString());

            Order order = client.Order.Create(options);


            if (!_currentUser.TenantId.HasValue)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            var payment = new Payment
            {
                TenantId = _currentUser.TenantId.Value,
                PlanId = plan.Id,
                TenantSubscriptionId = null,
                Amount = plan.Price,
                TransactionId = order["id"].ToString(),
                PaymentGateway = "Razorpay",
                PaymentStatus = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };
      
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

           

            return new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    OrderId = order["id"].ToString(),
                    Amount = plan.Price,
                    Key = _razorpay.KeyId
                }
            };
        }
        public async Task<ApiResponse<string>> VerifyPayment(VerifyPaymentDto model)
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

            if (model.Success)
            {
                payment.PaymentStatus = PaymentStatus.Success;

                var plan = await _context.Plans
                    .FirstOrDefaultAsync(x => x.Id == payment.PlanId);

                if (plan == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Plan not found"
                    };
                }

                var subscription = new TenantSubscription
                {
                    TenantId = payment.TenantId,
                    PlanId = payment.PlanId,
                    Amount = payment.Amount,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(plan.DurationInMonths),
                    SubscriptionStatus = "Active"
                };

                _context.TenantSubscriptions.Add(subscription);

                await _context.SaveChangesAsync();

                payment.TenantSubscriptionId = subscription.Id;

                await _context.SaveChangesAsync();
            }
            else
            {
                payment.PaymentStatus = PaymentStatus.Failed;

                await _context.SaveChangesAsync();
            }

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Payment verified successfully"
            };
        }
    }
}

