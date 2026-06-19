using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleListDto>>> GetAll(PaginationRequest request);
      
        ApiResponse<Role> Add(RoleCreateDto dto);
        ApiResponse<RoleUpdateDto> GetById(int id);
        ApiResponse<Role> Update(RoleUpdateDto dto);
        ApiResponse<string> Delete(int id);
        ApiResponse<string> ToggleStatus(int id);
    }
}
