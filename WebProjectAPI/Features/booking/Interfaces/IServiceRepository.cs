using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.booking.Models;

namespace WebProjectAPI.Features.booking.Interfaces
{
    public interface IServiceRepository
    {
        Task<ApiResponse<List<Service>>> GetAll(
            PaginationRequest request);

        Task<ApiResponse<Service>> GetById(int id);

        Task<ApiResponse<Service>> Add(Service model);

        Task<ApiResponse<Service>> Update(Service model);

        Task<ApiResponse<string>> Delete(int id);

        Task<ApiResponse<string>> ChangeStatus(int id);

        Task<List<Service>> Dropdown();
    }
}