using Microsoft.EntityFrameworkCore;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Infrastructure.Persistence;

public class VetsDbContext : DbContext
{
    public VetsDbContext(DbContextOptions<VetsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vet> Vets => Set<Vet>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<VetSpecialization> VetSpecializations => Set<VetSpecialization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetsDbContext).Assembly);
    }
}
