using AutoMapper;
using WebProjectAPI.DTOs;
using WebProjectAPI.DTOs.UserD;
using WebProjectAPI.DTOs.PermissionDto;
using WebProjectAPI.Models;
namespace WebProjectAPI.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<Role, RoleUpdateDto>();

            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        
            CreateMap<PermissionCreateDto, Permission>();
            CreateMap<PermissionUpdateDto, Permission>();
            CreateMap<Permission, PermissionUpdateDto>();
        }
    }
}
