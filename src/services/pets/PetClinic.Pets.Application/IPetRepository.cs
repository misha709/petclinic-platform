using PetClinic.Pets.Domain.Entities;

namespace PetClinic.Pets.Application;

public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Pet>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Pet> AddAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<Pet?> UpdateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
