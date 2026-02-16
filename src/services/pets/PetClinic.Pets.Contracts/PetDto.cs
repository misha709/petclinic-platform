namespace PetClinic.Pets.Contracts;

public record PetDto(
    Guid Id,
    string Name,
    string PetType,
    string Breed,
    DateTime? BirthDate,
    Guid OwnerId,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
