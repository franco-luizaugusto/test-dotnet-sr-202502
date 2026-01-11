using ApplicantTracking.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Abstractions;

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate?> GetByIdAsync(int idCandidate, CancellationToken cancellationToken);
    Task<List<Candidate>> ListAsync(CancellationToken cancellationToken);
    Task<(List<Candidate> Items, int TotalCount)> ListPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}

