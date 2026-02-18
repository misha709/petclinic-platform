using Microsoft.EntityFrameworkCore;
using PetClinic.Vets.Application;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Infrastructure.Persistence;

public class VetRepository : IVetRepository
{
    private readonly VetsDbContext _db;

    public VetRepository(VetsDbContext db)
    {
        _db = db;
    }

    public async Task<Vet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Vets.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<Vet?> GetVetWithSpecializationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Vets
            .AsNoTracking()
            .Include(v => v.VetSpecializations)
                .ThenInclude(vs => vs.Specialization)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Vet>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var queryable = _db.Vets
            .AsNoTracking()
            .Include(v => v.VetSpecializations)
                .ThenInclude(vs => vs.Specialization);

        if (string.IsNullOrWhiteSpace(query))
            return await queryable.OrderBy(v => v.LastName).ThenBy(v => v.FirstName).ToListAsync(cancellationToken);

        var term = query.Trim().ToLower();
        return await queryable
            .Where(v =>
                EF.Functions.ILike(v.FirstName, $"%{term}%") ||
                EF.Functions.ILike(v.LastName, $"%{term}%"))
            .OrderBy(v => v.LastName)
            .ThenBy(v => v.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Vet> AddAsync(Vet vet, CancellationToken cancellationToken = default)
    {
        _db.Vets.Add(vet);
        await _db.SaveChangesAsync(cancellationToken);
        return vet;
    }

    public async Task<Vet?> UpdateAsync(Vet vet, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Vets.FindAsync([vet.Id], cancellationToken);
        if (existing is null)
            return null;

        existing.FirstName = vet.FirstName;
        existing.LastName = vet.LastName;
        existing.UpdatedAt = vet.UpdatedAt;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vet = await _db.Vets
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null, cancellationToken);

        if (vet == null)
            return false;

        vet.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Vets.AnyAsync(v => v.Id == id, cancellationToken);

    public async Task AssignSpecializationsAsync(Guid vetId, int[] specializationIds, CancellationToken cancellationToken = default)
    {
        var existingAssignments = await _db.VetSpecializations
            .Where(vs => vs.VetId == vetId)
            .ToListAsync(cancellationToken);

        _db.VetSpecializations.RemoveRange(existingAssignments);

        var newAssignments = specializationIds
            .Select(specId => new VetSpecialization
            {
                VetId = vetId,
                SpecializationId = specId,
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        _db.VetSpecializations.AddRange(newAssignments);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
