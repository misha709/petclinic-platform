using Microsoft.EntityFrameworkCore;
using PetClinic.Visits.Domain.Entities;

namespace PetClinic.Visits.Infrastructure.Persistence;

public class VisitsDbContext : DbContext
{
    public VisitsDbContext(DbContextOptions<VisitsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Visit> Visits => Set<Visit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VisitsDbContext).Assembly);
    }
}
