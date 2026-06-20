    using WebProjectAPI.Features.payments.DTOs;
    using global::WebProjectAPI.Features.Common.ApiResponse;
    using global::WebProjectAPI.Features.Common.Paginations;


    namespace WebProjectAPI.Features.payments.Interfaces
    {
        public interface IPaymentRepository
        {
            Task<ApiResponse<List<PaymentDto>>> GetAll(PaginationRequest request);

            Task<ApiResponse<PaymentDto>> GetById(int id);

            Task<ApiResponse<PaymentDto>> Create(CreatePaymentDto model);

            Task<ApiResponse<string>> UpdateStatus(UpdatePaymentStatusDto model);

            Task<ApiResponse<string>> Delete(int id);

            Task<ApiResponse<PaymentDto>> GetByTransaction(string transactionId);

        Task<ApiResponse<object>> CreateOrder(CreateOrderDto model);

        Task<ApiResponse<string>> VerifyPayment(VerifyPaymentDto model);
    }
    }

