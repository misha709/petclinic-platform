namespace PetClinic.Vets.Domain.Entities;

public class Vet
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<VetSpecialization> VetSpecializations { get; set; } = new List<VetSpecialization>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
