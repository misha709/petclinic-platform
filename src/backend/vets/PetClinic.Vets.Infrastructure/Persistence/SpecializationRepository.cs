using Microsoft.EntityFrameworkCore;
using PetClinic.Vets.Application;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Infrastructure.Persistence;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly VetsDbContext _db;

    public SpecializationRepository(VetsDbContext db)
    {
        _db = db;
    }

    public async Task<Specialization?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.Specializations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Specialization>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Specializations.AsNoTracking().OrderBy(s => s.Name).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Specialization>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var queryable = _db.Specializations.AsNoTracking();

        if (string.IsNullOrWhiteSpace(query))
            return await queryable.OrderBy(s => s.Name).ToListAsync(cancellationToken);

        var term = query.Trim().ToLower();
        return await queryable
            .Where(s => EF.Functions.ILike(s.Name, $"%{term}%"))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Specialization> AddAsync(Specialization specialization, CancellationToken cancellationToken = default)
    {
        _db.Specializations.Add(specialization);
        await _db.SaveChangesAsync(cancellationToken);
        return specialization;
    }

    public async Task<Specialization?> UpdateAsync(Specialization specialization, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Specializations.FindAsync([specialization.Id], cancellationToken);
        if (existing is null)
            return null;

        existing.Name = specialization.Name;
        existing.UpdatedAt = specialization.UpdatedAt;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var specialization = await _db.Specializations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);

        if (specialization == null)
            return false;

        specialization.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.Specializations.AnyAsync(s => s.Id == id, cancellationToken);

    public async Task<Specialization?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await _db.Specializations.AsNoTracking().FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
}
