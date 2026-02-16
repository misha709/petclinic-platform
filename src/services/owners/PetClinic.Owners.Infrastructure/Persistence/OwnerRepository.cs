using Microsoft.EntityFrameworkCore;
using PetClinic.Owners.Application;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Infrastructure.Persistence;

public class OwnerRepository : IOwnerRepository
{
    private readonly OwnersDbContext _db;

    public OwnerRepository(OwnersDbContext db)
    {
        _db = db;
    }

    public async Task<Owner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Owners.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Owner>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var set = _db.Owners.AsNoTracking();
        if (string.IsNullOrWhiteSpace(query))
            return await set.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToListAsync(cancellationToken);

        var term = query.Trim().ToLower();
        return await set
            .Where(o =>
                EF.Functions.ILike(o.FirstName, $"%{term}%") ||
                EF.Functions.ILike(o.LastName, $"%{term}%") ||
                EF.Functions.ILike(o.Telephone, $"%{term}%"))
            .OrderBy(o => o.LastName)
            .ThenBy(o => o.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Owner> AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        _db.Owners.Add(owner);
        await _db.SaveChangesAsync(cancellationToken);
        return owner;
    }

    public async Task<Owner?> UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Owners.FindAsync([owner.Id], cancellationToken);
        if (existing is null)
            return null;

        existing.FirstName = owner.FirstName;
        existing.LastName = owner.LastName;
        existing.Address = owner.Address;
        existing.City = owner.City;
        existing.Telephone = owner.Telephone;
        existing.UpdatedAt = owner.UpdatedAt;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Owners.AnyAsync(o => o.Id == id, cancellationToken);
}
