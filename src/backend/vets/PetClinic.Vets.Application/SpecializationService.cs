using PetClinic.Vets.Contracts;
using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Application;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _repository;

    public SpecializationService(ISpecializationRepository repository)
    {
        _repository = repository;
    }

    public async Task<SpecializationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var specialization = await _repository.GetByIdAsync(id, cancellationToken);
        return specialization is null ? null : MapToDto(specialization);
    }

    public async Task<IReadOnlyList<SpecializationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var specializations = await _repository.GetAllAsync(cancellationToken);
        return specializations.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<SpecializationDto>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var specializations = await _repository.SearchAsync(query, cancellationToken);
        return specializations.Select(MapToDto).ToList();
    }

    public async Task<SpecializationDto> CreateAsync(CreateSpecializationRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null)
        {
            throw new ArgumentException($"Specialization with name '{request.Name}' already exists.", nameof(request));
        }

        var specialization = new Specialization
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(specialization, cancellationToken);
        return MapToDto(created);
    }

    public async Task<SpecializationDto?> UpdateAsync(int id, UpdateSpecializationRequest request, CancellationToken cancellationToken = default)
    {
        var specialization = await _repository.GetByIdAsync(id, cancellationToken);
        if (specialization is null)
            return null;

        var existingWithName = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (existingWithName is not null && existingWithName.Id != id)
        {
            throw new ArgumentException($"Specialization with name '{request.Name}' already exists.", nameof(request));
        }

        specialization.Name = request.Name;
        specialization.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(specialization, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    private static SpecializationDto MapToDto(Specialization specialization) =>
        new(
            specialization.Id,
            specialization.Name,
            specialization.CreatedAt,
            specialization.UpdatedAt);
}
