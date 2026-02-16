using PetClinic.Owners.Contracts;
using PetClinic.Owners.Domain.Entities;

namespace PetClinic.Owners.Application;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _repository;

    public OwnerService(IOwnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<OwnerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var owner = await _repository.GetByIdAsync(id, cancellationToken);
        return owner is null ? null : MapToDto(owner);
    }

    public async Task<IReadOnlyList<OwnerDto>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var owners = await _repository.SearchAsync(query, cancellationToken);
        return owners.Select(MapToDto).ToList();
    }

    public async Task<OwnerDto> CreateAsync(CreateOwnerRequest request, CancellationToken cancellationToken = default)
    {
        var owner = new Owner
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            City = request.City,
            Telephone = request.Telephone,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(owner, cancellationToken);
        return MapToDto(created);
    }

    public async Task<OwnerDto?> UpdateAsync(Guid id, UpdateOwnerRequest request, CancellationToken cancellationToken = default)
    {
        var owner = await _repository.GetByIdAsync(id, cancellationToken);
        if (owner is null)
            return null;

        owner.FirstName = request.FirstName;
        owner.LastName = request.LastName;
        owner.Address = request.Address;
        owner.City = request.City;
        owner.Telephone = request.Telephone;
        owner.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(owner, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    private static OwnerDto MapToDto(Owner owner) =>
        new(
            owner.Id,
            owner.FirstName,
            owner.LastName,
            owner.Address,
            owner.City,
            owner.Telephone,
            owner.CreatedAt,
            owner.UpdatedAt);
}
