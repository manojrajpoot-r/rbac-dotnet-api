using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.PermissionDto;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;

namespace WebProjectAPI.Services.Interfaces
{
    public interface IPermissionService
    {
        ApiResponse<List<Permission>> GetAll(int pageNumber, int pageSize, string search);
        ApiResponse<PermissionUpdateDto> GetById(int id);
        ApiResponse<List<Permission>> Add(PermissionCreateDto dto);
        ApiResponse<Permission> Update(PermissionUpdateDto dto);
        ApiResponse<string> Delete(int id);
       

    }
}
