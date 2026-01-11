using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Abstractions;

public interface IUnitOfWork
{
    ICandidateRepository Candidates { get; }
    ITimelineRepository Timelines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}

