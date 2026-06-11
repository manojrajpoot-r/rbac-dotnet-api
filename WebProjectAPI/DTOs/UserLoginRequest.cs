namespace WebProjectAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class UserLoginRequest
    {
        [Required]
        public string Email { get; set; }=string.Empty;

        [Required]
        public string Password { get; set; }=string.Empty;

        public bool IsPlatformUser { get; set; }
    }
}
