using Microsoft.EntityFrameworkCore;
using PetClinic.Pets.Application;
using PetClinic.Pets.Domain.Entities;

namespace PetClinic.Pets.Infrastructure.Persistence;

public class PetRepository : IPetRepository
{
    private readonly PetsDbContext _db;

    public PetRepository(PetsDbContext db)
    {
        _db = db;
    }

    public async Task<Pet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Pets.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Pet>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
        await _db.Pets.AsNoTracking()
            .Where(p => p.OwnerId == ownerId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

    public async Task<Pet> AddAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        _db.Pets.Add(pet);
        await _db.SaveChangesAsync(cancellationToken);
        return pet;
    }

    public async Task<Pet?> UpdateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Pets.FindAsync([pet.Id], cancellationToken);
        if (existing is null)
            return null;

        existing.Name = pet.Name;
        existing.PetType = pet.PetType;
        existing.Breed = pet.Breed;
        existing.BirthDate = pet.BirthDate;
        existing.OwnerId = pet.OwnerId;
        existing.UpdatedAt = pet.UpdatedAt;

        await _db.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Pets.AnyAsync(p => p.Id == id, cancellationToken);
}
