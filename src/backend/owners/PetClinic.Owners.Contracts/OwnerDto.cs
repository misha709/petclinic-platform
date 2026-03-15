namespace PetClinic.Owners.Contracts;

public record OwnerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Address,
    string City,
    string Telephone,
    string? Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
