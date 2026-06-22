using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Tenants.DTOs;
using WebProjectAPI.Models;
using WebProjectAPI.Features.Common.ApiResponse;
namespace WebProjectAPI.Services.Interfaces
{
    public interface IUserService
    {
      

        Task<ApiResponse<List<UserListDto>>> GetAll(PaginationRequest request);
        ApiResponse<User> Add(UserCreateDto dto);
        ApiResponse<UserUpdateDto> GetById(int id);

        ApiResponse<User> Update(UserUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);

        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
