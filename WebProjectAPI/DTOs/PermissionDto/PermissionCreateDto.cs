namespace WebProjectAPI.DTOs.PermissionDto
{
    using System.ComponentModel.DataAnnotations;
    public class PermissionCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
