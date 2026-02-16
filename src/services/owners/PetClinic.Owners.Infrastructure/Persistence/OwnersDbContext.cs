using Microsoft.EntityFrameworkCore;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Infrastructure.Persistence;

public class OwnersDbContext : DbContext
{
    public OwnersDbContext(DbContextOptions<OwnersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Owner> Owners => Set<Owner>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("owners");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(255).IsRequired();
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telephone).HasColumnName("telephone").HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasIndex(e => new { e.LastName, e.FirstName });
            entity.HasIndex(e => e.Telephone);
        });
    }
}
