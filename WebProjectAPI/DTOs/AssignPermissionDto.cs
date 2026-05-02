namespace WebProjectAPI.DTOs
{

    using System.ComponentModel.DataAnnotations;
    public class AssignPermissionDto
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public List<int> PermissionIds { get; set; }
    }
}
