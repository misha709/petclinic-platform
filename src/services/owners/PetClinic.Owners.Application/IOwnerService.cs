using PetClinic.Owners.Contracts;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Application;

public interface IOwnerService
{
    Task<OwnerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OwnerDto>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<OwnerDto> CreateAsync(CreateOwnerRequest request, CancellationToken cancellationToken = default);
    Task<OwnerDto?> UpdateAsync(Guid id, UpdateOwnerRequest request, CancellationToken cancellationToken = default);
}
