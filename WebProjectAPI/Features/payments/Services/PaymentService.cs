
    using WebProjectAPI.Features.Common.ApiResponse;
    using WebProjectAPI.Features.Common.Paginations;
    using WebProjectAPI.Features.payments.DTOs;
    using WebProjectAPI.Features.payments.Interfaces;
    using WebProjectAPI.Features.payments.Repositories;

    namespace WebProjectAPI.Features.payments.Services
    {
        public class PaymentService : IPaymentService
        {
            private readonly IPaymentRepository _paymentRepository;

            public PaymentService(IPaymentRepository paymentRepository)
            {
                _paymentRepository = paymentRepository;
            }

            public async Task<ApiResponse<List<PaymentDto>>> GetAll(PaginationRequest request)
            {
                return await _paymentRepository.GetAll(request);
            }

            public async Task<ApiResponse<PaymentDto>> GetById(int id)
            {
                return await _paymentRepository.GetById(id);
            }

            public async Task<ApiResponse<PaymentDto>> Create(CreatePaymentDto model)
            {
                return await _paymentRepository.Create(model);
            }

            public async Task<ApiResponse<string>> UpdateStatus(UpdatePaymentStatusDto model)
            {
                return await _paymentRepository.UpdateStatus(model);
            }

            public async Task<ApiResponse<string>> Delete(int id)
            {
                return await _paymentRepository.Delete(id);
            }

            public async Task<ApiResponse<PaymentDto>> GetByTransaction(string transactionId)
            {
                return await _paymentRepository.GetByTransaction(transactionId);
            }


        public async Task<ApiResponse<object>> CreateOrder(CreateOrderDto model)
        {
            return await _paymentRepository.CreateOrder(model);
        }

        public async Task<ApiResponse<string>> VerifyPayment(VerifyPaymentDto model)
        {
            return await _paymentRepository.VerifyPayment(model);
        }
    
    }
    }
