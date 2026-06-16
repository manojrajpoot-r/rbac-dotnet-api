using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IRoleService
    {
        ApiResponse<List<Role>> GetAll(PaginationRequest request);
        ApiResponse<Role> Add(RoleCreateDto dto);
        ApiResponse<RoleUpdateDto> GetById(int id);
        ApiResponse<Role> Update(RoleUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);
    }
}
