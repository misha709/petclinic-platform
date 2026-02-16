using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Infrastructure.Persistence.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> entity)
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
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        entity.HasIndex(e => new { e.LastName, e.FirstName });
        entity.HasIndex(e => e.Telephone);
        entity.HasIndex(e => e.DeletedAt);

        // Global query filter to exclude soft-deleted records
        entity.HasQueryFilter(o => o.DeletedAt == null);
    }
}
