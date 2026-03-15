using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetClinic.Visits.Domain.Entities;

namespace PetClinic.Visits.Infrastructure.Persistence.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> entity)
    {
        entity.ToTable("visits");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.PetId).HasColumnName("pet_id").IsRequired();
        entity.Property(e => e.VetId).HasColumnName("vet_id").IsRequired();
        entity.Property(e => e.ScheduledAt).HasColumnName("scheduled_at").IsRequired();
        entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes").IsRequired();
        entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(500).IsRequired();
        entity.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(1000);
        entity.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<int>();
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        entity.HasIndex(e => e.PetId);
        entity.HasIndex(e => e.VetId);
        entity.HasIndex(e => e.ScheduledAt);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.DeletedAt);

        entity.HasQueryFilter(v => v.DeletedAt == null);
    }
}
