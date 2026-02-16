using Microsoft.EntityFrameworkCore;
using PetClinic.Pets.Domain.Entities;

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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetsDbContext).Assembly);
    }
}
