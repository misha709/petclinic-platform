namespace PetClinic.Pets.Contracts;

public record UpdatePetRequest(
    string Name,
    string PetType,
    string Breed,
    DateTime? BirthDate,
    Guid OwnerId);
