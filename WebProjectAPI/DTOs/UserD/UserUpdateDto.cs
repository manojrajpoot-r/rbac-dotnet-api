using System.ComponentModel.DataAnnotations;

namespace WebProjectAPI.DTOs.UserD
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

    }
}
