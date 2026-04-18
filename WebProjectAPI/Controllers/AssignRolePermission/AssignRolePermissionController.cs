using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs;
using WebProjectAPI.Models;

namespace WebProjectAPI.Controllers.AssignRolePermission
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AssignRolePermissionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AssignRolePermissionController(AppDbContext context) {
            _context = context;
        }

        [HttpPost("assign-permission")]
        public IActionResult AssignPermission(AssignPermissionDto dto)
        {
            var exists = _context.RolePermissions
                .Any(x => x.RoleId == dto.RoleId && x.PermissionId == dto.PermissionId);

            if (exists)
                return BadRequest("Already assigned");

            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId
            });

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message ="Permission Assigned Successfully!"
            });
        }

        [HttpDelete("remove-permission")]
        public IActionResult RemovePermission(int roleId, int permissionId)
        {
            var data = _context.RolePermissions
                .FirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId);

            if (data == null)
                return NotFound("Not found");

            _context.RolePermissions.Remove(data);
            _context.SaveChanges();

            return Ok(new
            {
               success=true,
               message= "Permission Removed Successfully!"
                
            });
        }

      
        [HttpGet("role-permissions/{roleId}")]
        public IActionResult GetRolePermissions(int roleId)
        {
            var permissions = _context.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Select(x => x.Permission.Name)
                .ToList();

            return Ok(new
            {
                success = true,
                data =permissions
            });
               
        }

        [HttpGet("user-permissions/{userId}")]
        public IActionResult GetUserPermissions(int userId)
        {
            var permissions = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(_context.Permissions,
                    rp => rp,
                    p => p.Id,
                    (rp, p) => p.Name)
                .Distinct()
                .ToList();

            return Ok(permissions);
        }
    }
}
