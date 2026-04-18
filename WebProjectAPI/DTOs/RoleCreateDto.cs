namespace WebProjectAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class RoleCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
