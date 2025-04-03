namespace EFCoreTPCFilters.Entities
{
    public class TenantUser : User
    {
        public required Guid TenantId { get; set; }
        public required string Department { get; set; }
    }
}
