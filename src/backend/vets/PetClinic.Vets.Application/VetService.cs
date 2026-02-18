using PetClinic.Vets.Contracts;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Application;

public class VetService : IVetService
{
    private readonly IVetRepository _repository;

    public VetService(IVetRepository repository)
    {
        _repository = repository;
    }

    public async Task<VetDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vet = await _repository.GetVetWithSpecializationsAsync(id, cancellationToken);
        return vet is null ? null : MapToDto(vet);
    }

    public async Task<IReadOnlyList<VetDto>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var vets = await _repository.SearchAsync(query, cancellationToken);
        return vets.Select(MapToDto).ToList();
    }

    public async Task<VetDto> CreateAsync(CreateVetRequest request, CancellationToken cancellationToken = default)
    {
        var vet = new Vet
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(vet, cancellationToken);
        return MapToDto(created);
    }

    public async Task<VetDto?> UpdateAsync(Guid id, UpdateVetRequest request, CancellationToken cancellationToken = default)
    {
        var vet = await _repository.GetByIdAsync(id, cancellationToken);
        if (vet is null)
            return null;

        vet.FirstName = request.FirstName;
        vet.LastName = request.LastName;
        vet.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(vet, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<VetDto?> AssignSpecializationsAsync(Guid id, AssignSpecializationsRequest request, CancellationToken cancellationToken = default)
    {
        var vet = await _repository.GetByIdAsync(id, cancellationToken);
        if (vet is null)
            return null;

        await _repository.AssignSpecializationsAsync(id, request.SpecializationIds, cancellationToken);
        
        var updated = await _repository.GetVetWithSpecializationsAsync(id, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    private static VetDto MapToDto(Vet vet) =>
        new(
            vet.Id,
            vet.FirstName,
            vet.LastName,
            vet.VetSpecializations
                .Select(vs => new SpecializationDto(
                    vs.Specialization.Id,
                    vs.Specialization.Name,
                    vs.Specialization.CreatedAt,
                    vs.Specialization.UpdatedAt))
                .ToArray(),
            vet.CreatedAt,
            vet.UpdatedAt);
}
