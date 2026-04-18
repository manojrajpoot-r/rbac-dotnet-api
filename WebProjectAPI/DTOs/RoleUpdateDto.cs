namespace WebProjectAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class RoleUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
