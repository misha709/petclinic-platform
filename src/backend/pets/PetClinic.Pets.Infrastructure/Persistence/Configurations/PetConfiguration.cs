using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetClinic.Pets.Domain.Entities;

namespace PetClinic.Pets.Infrastructure.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> entity)
    {
        entity.ToTable("pets");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        entity.Property(e => e.PetType)
            .HasColumnName("pet_type")
            .IsRequired()
            .HasConversion<int>();
        entity.Property(e => e.Breed).HasColumnName("breed").HasMaxLength(100).IsRequired();
        entity.Property(e => e.BirthDate).HasColumnName("birth_date");
        entity.Property(e => e.OwnerId).HasColumnName("owner_id");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        entity.HasIndex(e => e.OwnerId);
        entity.HasIndex(e => e.DeletedAt);

        // Global query filter to exclude soft-deleted records
        entity.HasQueryFilter(p => p.DeletedAt == null);
    }
}
