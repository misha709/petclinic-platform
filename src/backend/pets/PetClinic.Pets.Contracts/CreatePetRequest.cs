namespace PetClinic.Pets.Contracts;

public record CreatePetRequest(
    string Name,
    string PetType,
    string Breed,
    DateTime? BirthDate,
    Guid OwnerId);
