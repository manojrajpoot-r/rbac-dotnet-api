using WebProjectAPI.Features.booking.DTOs;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;

namespace WebProjectAPI.Features.booking.Interfaces
{
    public interface IServiceService
    {
        Task<ApiResponse<List<Service>>> GetAll(
            PaginationRequest request);

        Task<ApiResponse<Service>> GetById(int id);

        Task<ApiResponse<Service>> Add(
            CreateServiceDto model);

        Task<ApiResponse<Service>> Update(
            UpdateServiceDto model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<ApiResponse<List<Service>>> Dropdown();
    }
}