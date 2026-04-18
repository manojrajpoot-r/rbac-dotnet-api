namespace WebProjectAPI.Services.Interfaces
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using WebProjectAPI.DTOs.UserD;
    using WebProjectAPI.Helpers;
    using WebProjectAPI.Models;
    using WebProjectAPI.Repositories.Interfaces;
    using WebProjectAPI.Services.Implementations;

    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _hasher;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _hasher = new PasswordHasher<User>();
        }

        public ApiResponse<List<User>> GetAll()
        {
            return new ApiResponse<List<User>>
            {
                Success = true,
                Data = _repo.GetAll()
            };
        }

        public ApiResponse<User> Add(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.Password = _hasher.HashPassword(user, dto.Password);

            var result = _repo.Add(user);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User created",
                Data = result
            };
        }

        public ApiResponse<User> Update(UserUpdateDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var result = _repo.Update(user);

            if (result == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            return new ApiResponse<User>
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
