namespace WebProjectAPI.Services.Interfaces
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using WebProjectAPI.DTOs.UserD;
    using WebProjectAPI.Features.Common.Paginations;
    using WebProjectAPI.Models;
    using WebProjectAPI.Repositories.Interfaces;
    using WebProjectAPI.Features.Common.ApiResponse;
    using WebProjectAPI.Data;
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _hasher;
        private readonly AppDbContext _context;

        public UserService(IUserRepository repo, IMapper mapper, AppDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _hasher = new PasswordHasher<User>();
            _context = context;
        }


        public async Task<ApiResponse<List<UserListDto>>> GetAll(PaginationRequest request)
        {
            return await _repo.GetAll(request);
        }
 

        public ApiResponse<User> Add(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            var result = _repo.Add(user);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User created",
                Data = result
            };
        }
        public ApiResponse<UserUpdateDto> GetById(int id)
        {
            var user = _repo.GetById(id);

            if (user == null)
            {
                return new ApiResponse<UserUpdateDto>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var dto = _mapper.Map<UserUpdateDto>(user);

            return new ApiResponse<UserUpdateDto>
            {
                Success = true,
                Data = dto
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
                Data = result,
                Message = "User updated successfully"
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

        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "User not found",
                    Data = false
                };
            }

            var verifyResult = _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.CurrentPassword
            );

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Current password is incorrect",
                    Data = false
                };
            }

            user.PasswordHash = _hasher.HashPassword(
                user,
                request.NewPassword
            );

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Password changed successfully",
                Data = true
            };
        }
        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            user.PasswordHash = _hasher.HashPassword(
                user,
                request.NewPassword
            );

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Password reset successfully",
                Data = true
            };
        }


    }
}
