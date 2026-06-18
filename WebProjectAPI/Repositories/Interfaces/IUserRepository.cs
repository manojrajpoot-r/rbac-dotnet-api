using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Models;
using WebProjectAPI.Features.Common.ApiResponse;
namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
       

        Task<ApiResponse<List<UserListDto>>> GetAll(PaginationRequest request);
        User GetById(int id);
        User Add(User user);
        User Update(User user);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
