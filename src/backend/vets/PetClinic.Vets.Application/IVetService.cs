using PetClinic.Vets.Contracts;

namespace PetClinic.Vets.Application;

public interface IVetService
{
    Task<VetDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VetDto>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<VetDto> CreateAsync(CreateVetRequest request, CancellationToken cancellationToken = default);
    Task<VetDto?> UpdateAsync(Guid id, UpdateVetRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VetDto?> AssignSpecializationsAsync(Guid id, AssignSpecializationsRequest request, CancellationToken cancellationToken = default);
}
