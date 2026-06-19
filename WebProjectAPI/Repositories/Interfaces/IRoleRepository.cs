using WebProjectAPI.DTOs;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Models;

namespace WebProjectAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<ApiResponse<List<RoleListDto>>> GetAll(PaginationRequest request);
     
        Role GetById(int id);
        Role Add(Role role);
        Role Update(Role role);
        bool Delete(int id);
        bool ToggleStatus(int id);
    }
}
