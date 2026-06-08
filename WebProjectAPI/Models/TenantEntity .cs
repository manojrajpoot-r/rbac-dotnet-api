namespace WebProjectAPI.Models
{
    public abstract class TenantEntity : BaseEntity
    {
        public Guid TenantId { get; set; }

        public bool IsDeleted { get; set; }
    }
}