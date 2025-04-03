using EFCoreTPCFilters.Entities;
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

namespace EFCoreTPCFilters
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<PublicUser> PublicUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Email);
                b.UseTpcMappingStrategy();
            });

            modelBuilder.Entity<TenantUser>();
            modelBuilder.ApplyQueryFilter<TenantUser>(u => !TenantContext.IsTenant || u.TenantId == TenantContext.TenantIdOrEmpty);
            modelBuilder.Entity<PublicUser>();
        }
    }
}
