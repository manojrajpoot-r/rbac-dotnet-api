namespace WebProjectAPI.DTOs.PermissionDto
{
    using System.ComponentModel.DataAnnotations;
    public class PermissionCreateDto
    {
        [Required]
        public List<string> Permissions { get; set; }

        [Required]
        public string GroupName { get; set; }
      
    }
}
