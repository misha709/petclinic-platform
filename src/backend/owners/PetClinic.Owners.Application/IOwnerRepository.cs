using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Application;

public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Owner>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<Owner> AddAsync(Owner owner, CancellationToken cancellationToken = default);
    Task<Owner?> UpdateAsync(Owner owner, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
