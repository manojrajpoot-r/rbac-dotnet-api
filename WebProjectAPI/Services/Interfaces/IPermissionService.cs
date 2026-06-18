using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.PermissionDto;
using WebProjectAPI.Features.Common.Paginations;
using WebProjectAPI.Features.Common.ApiResponse;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IPermissionService
    {
        ApiResponse<List<Permission>> GetAll(PaginationRequest request);
        ApiResponse<PermissionUpdateDto> GetById(int id);
        ApiResponse<List<Permission>> Add(PermissionCreateDto dto);
        ApiResponse<Permission> Update(PermissionUpdateDto dto);
        ApiResponse<string> Delete(int id);
       

    }
}
