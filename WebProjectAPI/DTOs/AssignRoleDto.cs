namespace WebProjectAPI.DTOs
{

    using System.ComponentModel.DataAnnotations;
    public class AssignRoleDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public List<int> RoleIds { get; set; }
    }
}
