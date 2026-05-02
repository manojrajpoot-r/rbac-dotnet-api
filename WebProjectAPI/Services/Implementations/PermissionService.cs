using AutoMapper;
using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.PermissionDto;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public ApiResponse<List<Permission>> GetAll(int pageNumber, int pageSize, string search)
        {
            int totalRecords;

            var data = _repo.GetAll(pageNumber, pageSize, search, out totalRecords);

            return new ApiResponse<List<Permission>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public ApiResponse<PermissionUpdateDto> GetById(int id)
        {
            var permission = _repo.GetById(id);

            if (permission == null)
            {
                return new ApiResponse<PermissionUpdateDto>
                {
                    Success = false,
                    Message = "Permission not found"
                };
            }

            var dto = _mapper.Map<PermissionUpdateDto>(permission);

            return new ApiResponse<PermissionUpdateDto>
            {
                Success = true,
                Data = dto
            };
        }

        public ApiResponse<List<Permission>> Add(PermissionCreateDto dto)
        {
            List<Permission> addedPermissions = new();

            foreach (var item in dto.Permissions)
            {
                var permission = new Permission
                {
                    Name = item,
                    GroupName = dto.GroupName
                };

                _repo.Add(permission);

                addedPermissions.Add(permission);
            }

            return new ApiResponse<List<Permission>>
            {
                Success = true,
                Message = "Permissions added successfully",
                Data = addedPermissions
            };
        }

        public ApiResponse<Permission> Update(PermissionUpdateDto dto)
        {
            var p = _mapper.Map<Permission>(dto);
            var result = _repo.Update(p);

            if (result == null)
            {
                return new ApiResponse<Permission>
                {
                    Success = false,
                    Message = "Permission not found"
                };
            }

            return new ApiResponse<Permission>
            {
                Success = true,
                Message = "Permission Updated",
                Data = result
            };
        }

        public ApiResponse<string> Delete(int id)
        {
            var result = _repo.Delete(id);

            return new ApiResponse<string>
            {
                Success = result,
                Message = result ? "Permission Deleted" : "Not found"
            };
        }

        
    }
}

