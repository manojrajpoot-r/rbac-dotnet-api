namespace WebProjectAPI.Models
{
    public abstract class TenantEntity : BaseEntity
    {
        public int TenantId { get; set; }

        public bool IsDeleted { get; set; }
    }
}