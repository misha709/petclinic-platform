namespace PetClinic.Vets.Domain.Entities;

public class Specialization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<VetSpecialization> VetSpecializations { get; set; } = new List<VetSpecialization>();
}
