using PetClinic.Pets.Contracts;

namespace PetClinic.Pets.Application;

public interface IPetService
{
    Task<PetDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PetDto>> SearchAsync(string? query, Guid? ownerId, CancellationToken cancellationToken = default);
    Task<PetDto> CreateAsync(CreatePetRequest request, CancellationToken cancellationToken = default);
    Task<PetDto?> UpdateAsync(Guid id, UpdatePetRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
