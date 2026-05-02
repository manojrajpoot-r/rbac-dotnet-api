using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IRoleService
    {
        ApiResponse<List<Role>> GetAll(int pageNumber, int pageSize, string search);
        ApiResponse<Role> Add(RoleCreateDto dto);
        ApiResponse<RoleUpdateDto> GetById(int id);
        ApiResponse<Role> Update(RoleUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);
    }
}
