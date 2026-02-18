namespace PetClinic.Vets.Contracts;

public record SpecializationDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
