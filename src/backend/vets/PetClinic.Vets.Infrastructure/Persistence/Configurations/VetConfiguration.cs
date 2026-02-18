using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Infrastructure.Persistence.Configurations;

public class VetConfiguration : IEntityTypeConfiguration<Vet>
{
    public void Configure(EntityTypeBuilder<Vet> entity)
    {
        entity.ToTable("vets");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        entity.HasIndex(e => new { e.LastName, e.FirstName });
        entity.HasIndex(e => e.DeletedAt);

        entity.HasMany(v => v.VetSpecializations)
            .WithOne(vs => vs.Vet)
            .HasForeignKey(vs => vs.VetId);

        entity.HasQueryFilter(v => v.DeletedAt == null);
    }
}
