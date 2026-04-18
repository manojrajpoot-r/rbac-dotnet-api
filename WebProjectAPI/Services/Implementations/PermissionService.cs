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

        public ApiResponse<List<Permission>> GetAll()
        {
            return new ApiResponse<List<Permission>>
            {
                Success = true,
                Data = _repo.GetAll()
            };
        }

        public ApiResponse<Permission> Add(PermissionCreateDto dto)
        {
            var p = _mapper.Map<Permission>(dto);
            var result = _repo.Add(p);

            return new ApiResponse<Permission>
            {
                Success = true,
                Message = "Permission added",
                Data = result
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

