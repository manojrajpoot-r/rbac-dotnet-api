namespace WebProjectAPI.DTOs
{
    public class JwtResult
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
