using PetClinic.Visits.Domain.Entities;
using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Application;

public interface IVisitRepository
{
    Task<Visit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Visit>> SearchAsync(Guid? petId, Guid? vetId, DateOnly? date, VisitStatus? status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Visit>> GetOverlappingAsync(Guid vetId, DateTime start, DateTime end, Guid? excludeId, CancellationToken cancellationToken = default);
    Task<Visit> AddAsync(Visit visit, CancellationToken cancellationToken = default);
    Task<Visit?> UpdateAsync(Visit visit, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
