using PetClinic.Visits.Contracts;
using PetClinic.Visits.Domain.Enums;

namespace PetClinic.Visits.Application;

public interface IVisitService
{
    Task<VisitDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VisitDto>> SearchAsync(Guid? petId, Guid? vetId, DateOnly? date, VisitStatus? status, CancellationToken cancellationToken = default);
    Task<VisitDto> CreateAsync(CreateVisitRequest request, CancellationToken cancellationToken = default);
    Task<VisitDto?> UpdateAsync(Guid id, UpdateVisitRequest request, CancellationToken cancellationToken = default);
    Task<VisitDto?> CancelAsync(Guid id, CancelVisitRequest request, CancellationToken cancellationToken = default);
    Task<VisitDto?> CompleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
