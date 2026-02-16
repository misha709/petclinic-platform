namespace PetClinic.Pets.Domain.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PetType { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
