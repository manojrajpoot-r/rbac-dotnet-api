using WebProjectAPI.DTOs;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IRoleService
    {
        ApiResponse<List<Role>> GetAll();
        ApiResponse<Role> Add(RoleCreateDto dto);
        ApiResponse<Role> Update(RoleUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);
    }
}
