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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OwnersDbContext).Assembly);
    }
}
