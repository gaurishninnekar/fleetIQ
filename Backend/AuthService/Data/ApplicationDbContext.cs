using System;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthService.Data;

public class ApplicationDbContext : IdentityDbContext<TenantUser>
{
    public DbSet<Tenant> Tenants { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tenant>(entity => 
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired();
        });

        builder.Entity<TenantUser>(entity =>
        {
            entity.HasOne(u => u.Tenant)
                  .WithMany(t => t.Users)
                  .HasForeignKey(u => u.TenantId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
