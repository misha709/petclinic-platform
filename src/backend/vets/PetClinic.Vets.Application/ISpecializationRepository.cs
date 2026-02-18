using PetClinic.Vets.Domain.Entities;

namespace PetClinic.Vets.Application;

public interface ISpecializationRepository
{
    Task<Specialization?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Specialization>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Specialization>> SearchAsync(string? query, CancellationToken cancellationToken = default);
    Task<Specialization> AddAsync(Specialization specialization, CancellationToken cancellationToken = default);
    Task<Specialization?> UpdateAsync(Specialization specialization, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Specialization?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
