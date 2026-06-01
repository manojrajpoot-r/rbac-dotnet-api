using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<List<BookingResponseDto>>> GetAll(
       PaginationRequest request);
        Task<ApiResponse<BookingResponseDto>> GetById(int id);

        Task<ApiResponse<BookingResponseDto>> Add(
            CreateBookingDto model);

        Task<ApiResponse<Booking>> Update(
            UpdateBookingDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<ApiResponse<List<BookingResponseDto>>> GetByUser(int userId);
    }
}