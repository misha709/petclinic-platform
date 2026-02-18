using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Application;

public interface IVetRepository
{
    Task<Vet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vet>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<Vet> AddAsync(Vet vet, CancellationToken cancellationToken = default);
    Task<Vet?> UpdateAsync(Vet vet, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Vet?> GetVetWithSpecializationsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AssignSpecializationsAsync(Guid vetId, int[] specializationIds, CancellationToken cancellationToken = default);
}
