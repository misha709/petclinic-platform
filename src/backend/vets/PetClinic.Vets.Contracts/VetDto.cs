namespace PetClinic.Vets.Contracts;

public record VetDto(
    Guid Id,
    string FirstName,
    string LastName,
    SpecializationDto[] Specializations,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
