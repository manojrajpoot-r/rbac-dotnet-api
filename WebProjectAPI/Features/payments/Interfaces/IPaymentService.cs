
    using global::WebProjectAPI.Features.Common.ApiResponse;
    using global::WebProjectAPI.Features.Common.Paginations;
    using global::WebProjectAPI.Features.payments.DTOs;
    namespace WebProjectAPI.Features.payments.Interfaces
    {
        public interface IPaymentService
        {
            Task<ApiResponse<List<PaymentDto>>> GetAll(PaginationRequest request);

            Task<ApiResponse<PaymentDto>> GetById(int id);

            Task<ApiResponse<PaymentDto>> Create(CreatePaymentDto model);

            Task<ApiResponse<string>> UpdateStatus(UpdatePaymentStatusDto model);

            Task<ApiResponse<string>> Delete(int id);

            Task<ApiResponse<PaymentDto>> GetByTransaction(string transactionId);
        }
    }

