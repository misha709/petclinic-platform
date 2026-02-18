using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Infrastructure.Persistence.Configurations;

public class VetSpecializationConfiguration : IEntityTypeConfiguration<VetSpecialization>
{
    public void Configure(EntityTypeBuilder<VetSpecialization> entity)
    {
        entity.ToTable("vet_specializations");
        entity.HasKey(e => new { e.VetId, e.SpecializationId });
        
        entity.Property(e => e.VetId).HasColumnName("vet_id");
        entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");

        entity.HasOne(e => e.Vet)
            .WithMany(v => v.VetSpecializations)
            .HasForeignKey(e => e.VetId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Specialization)
            .WithMany(s => s.VetSpecializations)
            .HasForeignKey(e => e.SpecializationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
