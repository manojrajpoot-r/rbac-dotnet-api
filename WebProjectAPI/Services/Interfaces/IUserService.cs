using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IUserService
    {
        ApiResponse<List<User>> GetAll();
        ApiResponse<User> Add(UserCreateDto dto);
        ApiResponse<UserUpdateDto> GetById(int id);

        ApiResponse<User> Update(UserUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);
    }
}
