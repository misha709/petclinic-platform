using PetClinic.Vets.Contracts;

namespace PetClinic.Vets.Application;

public interface ISpecializationService
{
    Task<SpecializationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SpecializationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SpecializationDto>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<SpecializationDto> CreateAsync(CreateSpecializationRequest request, CancellationToken cancellationToken = default);
    Task<SpecializationDto?> UpdateAsync(int id, UpdateSpecializationRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
