using AutoMapper;
using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.Helpers;
using WebProjectAPI.Models;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public ApiResponse<List<Role>> GetAll(int pageNumber, int pageSize, string search)
        {
            int totalRecords;

            var data = _repo.GetAll(pageNumber, pageSize, search, out totalRecords);

            return new ApiResponse<List<Role>>
            {
                Success = true,
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber, 
                PageSize = pageSize      
            };
        }

        public ApiResponse<Role> Add(RoleCreateDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            var result = _repo.Add(role);

            return new ApiResponse<Role>
            {
                Success = true,
                Message = "Role added",
                Data = result
            };
        }


        public ApiResponse<RoleUpdateDto> GetById(int id)
        {
            var role = _repo.GetById(id);

            if (role == null)
            {
                return new ApiResponse<RoleUpdateDto>
                {
                    Success = false,
                    Message = "Role not found"
                };
            }

            var dto = _mapper.Map<RoleUpdateDto>(role);

            return new ApiResponse<RoleUpdateDto>
            {
                Success = true,
                Data = dto
            };
        }
        public ApiResponse<Role> Update(RoleUpdateDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            var result = _repo.Update(role);

            if (result == null)
            {
                return new ApiResponse<Role>
                {
                    Success = false,
                    Message = "Role not found"
                };
            }

            return new ApiResponse<Role>
            {
                Success = true,
                Message = "Updated",
                Data = result
            };
        }

        public ApiResponse<string> Delete(int id)
        {
            var result = _repo.Delete(id);

            return new ApiResponse<string>
            {
                Success = result,
                Message = result ? "Deleted" : "Not found"
            };
        }

        public ApiResponse<string> ToggleStatus(int id)
        {
            var result = _repo.ToggleStatus(id);

            return new ApiResponse<string>
            {
                Success = result,
                Message = result ? "Status changed" : "Not found"
            };
        }
    }
}
