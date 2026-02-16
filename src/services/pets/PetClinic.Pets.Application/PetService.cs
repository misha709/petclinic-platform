using PetClinic.Pets.Contracts;
using PetClinic.Pets.Domain.Entities;

namespace PetClinic.Pets.Application;

public class PetService : IPetService
{
    private readonly IPetRepository _repository;

    public PetService(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PetDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pet = await _repository.GetByIdAsync(id, cancellationToken);
        return pet is null ? null : MapToDto(pet);
    }

    public async Task<IReadOnlyList<PetDto>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var pets = await _repository.GetByOwnerIdAsync(ownerId, cancellationToken);
        return pets.Select(MapToDto).ToList();
    }

    public async Task<PetDto> CreateAsync(CreatePetRequest request, CancellationToken cancellationToken = default)
    {
        var pet = new Pet
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            PetType = request.PetType,
            Breed = request.Breed,
            BirthDate = request.BirthDate,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(pet, cancellationToken);
        return MapToDto(created);
    }

    public async Task<PetDto?> UpdateAsync(Guid id, UpdatePetRequest request, CancellationToken cancellationToken = default)
    {
        var pet = await _repository.GetByIdAsync(id, cancellationToken);
        if (pet is null)
            return null;

        pet.Name = request.Name;
        pet.PetType = request.PetType;
        pet.Breed = request.Breed;
        pet.BirthDate = request.BirthDate;
        pet.OwnerId = request.OwnerId;
        pet.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(pet, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    private static PetDto MapToDto(Pet pet) =>
        new(
            pet.Id,
            pet.Name,
            pet.PetType,
            pet.Breed,
            pet.BirthDate,
            pet.OwnerId,
            pet.CreatedAt,
            pet.UpdatedAt);
}
