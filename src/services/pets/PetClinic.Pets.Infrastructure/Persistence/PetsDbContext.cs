using Microsoft.EntityFrameworkCore;
using PetClinic.Pets.Domain.Entities;
using PetClinic.Pets.Domain.Enums;

namespace PetClinic.Pets.Infrastructure.Persistence;

public class PetsDbContext : DbContext
{
    public PetsDbContext(DbContextOptions<PetsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets => Set<Pet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
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

            entity.HasIndex(e => e.OwnerId);
        });
    }
}
