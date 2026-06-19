namespace WebProjectAPI.DTOs.UserD
{
    public class UserListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Status { get; set; }

        public string TenantName { get; set; } = string.Empty;
    }

}
