using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs;
using WebProjectAPI.Models;

namespace WebProjectAPI.Controllers.AssignUserRole
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AssignUserRoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssignUserRoleController(AppDbContext context) { 
        
          _context = context;
        
        }

      
        [HttpPost("assign-role")]
        public IActionResult AssignRole(AssignRoleDto dto)
        {
            if (dto.RoleIds == null || !dto.RoleIds.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Please select at least one role"
                });
            }

            var existingRoles = _context.UserRoles
                .Where(x => x.UserId == dto.UserId)
                .ToList();

            _context.UserRoles.RemoveRange(existingRoles);

            var userRoles = dto.RoleIds.Select(roleId => new UserRole
            {
                UserId = dto.UserId,
                RoleId = roleId
            }).ToList();

            _context.UserRoles.AddRange(userRoles);

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Roles assigned successfully!"
            });
        }


        [HttpDelete("remove-role")]
        public IActionResult RemoveRole(int userId, int roleId)
        {
            var data = _context.UserRoles
                .FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);

            if (data == null)
                return NotFound("Not found");

            _context.UserRoles.Remove(data);
            _context.SaveChanges();

            return Ok(new
            {
               success=true,
               message= "Role Removed successfully!"
            });
        }

        [HttpGet("user-roles/{userId}")]
        public IActionResult GetUserRoles(int userId)
        {
            var roles = _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.Role.Name)
                .ToList();

            return Ok(roles);
        }
    }
}
