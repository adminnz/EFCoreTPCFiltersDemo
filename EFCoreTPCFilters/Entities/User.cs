namespace EFCoreTPCFilters.Entities
{
    public abstract class User
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string PasswordHash { get; set; }
    }
}
