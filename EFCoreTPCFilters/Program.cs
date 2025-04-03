using EFCoreTPCFilters;
using EFCoreTPCFilters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Test"));
    }).Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    //seed with public users
    context.PublicUsers.Add(new PublicUser { Email = "public1@domain.com", FirstName = "first", PasswordHash = "MyHashHere", Expires = new DateTime(2026, 1, 1) });
    context.PublicUsers.Add(new PublicUser { Email = "public2@domain.com", FirstName = "second", PasswordHash = "MyHashHere", Expires = new DateTime(2026, 1, 1) });
    //seed with tenant users
    var tenant1_id = Guid.NewGuid();
    context.TenantUsers.Add(new TenantUser { TenantId = tenant1_id, Email = "bob@tenant1.com", FirstName = "bob", PasswordHash = "Something", Department = "sales" });
    context.TenantUsers.Add(new TenantUser { TenantId = tenant1_id, Email = "jim@tenant1.com", FirstName = "jim", PasswordHash = "Something", Department = "customer support" });
    var tenant2_id = Guid.NewGuid();
    context.TenantUsers.Add(new TenantUser { TenantId = tenant2_id, Email = "tim@tenant2.com", FirstName = "tim", PasswordHash = "Something", Department = "customer support" });
    context.TenantUsers.Add(new TenantUser { TenantId = tenant2_id, Email = "jim@tenant2.com", FirstName = "jim", PasswordHash = "Something", Department = "human resources" });
    context.SaveChanges();

    context.ChangeTracker.Clear();
    Console.WriteLine("There are {0} Users, when not in Tenant scope", context.Users.Count());

    context.ChangeTracker.Clear();
    using (TenantContext.Scope(tenant1_id))
    {
        Console.WriteLine("There are {0} Users, when in Tenant {1} scope", context.Users.Count(), TenantContext.TenantId);
    }
    Console.ReadKey();
}