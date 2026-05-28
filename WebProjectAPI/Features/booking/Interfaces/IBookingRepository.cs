using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.booking.Models;

namespace WebProjectAPI.Features.booking.Interfaces
{
    public interface IBookingRepository
    {
        Task<ApiResponse<List<Booking>>> GetAll(
            PaginationRequest request);

        Task<ApiResponse<Booking>> GetById(int id);

        Task<ApiResponse<Booking>> Add(Booking model);

        Task<ApiResponse<Booking>> Update(Booking model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);
    }
}