using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<List<Booking>>> GetAll(
            PaginationRequest request);

        Task<ApiResponse<Booking>> GetById(int id);

        Task<ApiResponse<Booking>> Add(
            CreateBookingDto model);

        Task<ApiResponse<Booking>> Update(
            UpdateBookingDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);
    }
}