namespace PetClinic.Vets.Domain.Entities;

public class VetSpecialization
{
    public Guid VetId { get; set; }
    public int SpecializationId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Vet Vet { get; set; } = null!;
    public Specialization Specialization { get; set; } = null!;
}
