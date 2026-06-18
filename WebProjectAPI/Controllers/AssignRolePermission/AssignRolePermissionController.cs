using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data;
using WebProjectAPI.DTOs;
using WebProjectAPI.Models;

namespace WebProjectAPI.Controllers.AssignRolePermission
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignRolePermissionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssignRolePermissionController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Permission")]
        // ==========================
        // ASSIGN PERMISSIONS
        // ==========================
        [HttpPost("assign-permission")]
        public IActionResult AssignPermission([FromBody] AssignPermissionDto dto)
        {
            var role = _context.Roles
                .FirstOrDefault(x => x.Id == dto.RoleId);

            if (role == null)
                return NotFound("Role not found");

            // already assigned permissions (fast lookup)
            var existingPermissions = _context.RolePermissions
                .Where(x => x.RoleId == dto.RoleId)
                .Select(x => x.PermissionId)
                .ToHashSet();

            foreach (var permissionId in dto.PermissionIds)
            {
                if (!existingPermissions.Contains(permissionId))
                {
                    _context.RolePermissions.Add(new RolePermission
                    {
                        TenantId = role.TenantId,
                        RoleId = dto.RoleId,
                        PermissionId = permissionId
                    });
                }
            }

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Permissions Assigned Successfully!"
            });
        }

        // ==========================
        // REMOVE PERMISSION
        // ==========================
        [HttpDelete("remove-permission")]
        public IActionResult RemovePermission(int roleId, int permissionId)
        {
            var data = _context.RolePermissions
                .FirstOrDefault(x =>
                    x.RoleId == roleId &&
                    x.PermissionId == permissionId);

            if (data == null)
                return NotFound("Not found");

            _context.RolePermissions.Remove(data);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Permission Removed Successfully!"
            });
        }

        // ==========================
        // ROLE PERMISSIONS
        // ==========================
        [HttpGet("role-permissions/{roleId}")]
        public IActionResult GetRolePermissions(int roleId)
        {
            var permissions = _context.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Include(x => x.Permission)
                .Where(x => x.Permission != null)
                .Select(x => new
                {
                    id = x.PermissionId,
                    name = x.Permission!.Name
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = permissions
            });
        }

        // ==========================
        // USER PERMISSIONS
        // ==========================
        [HttpGet("user-permissions/{userId}")]
        public IActionResult GetUserPermissions(int userId)
        {
            var permissions = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();

            return Ok(new
            {
                success = true,
                data = permissions
            });
        }
    }
}