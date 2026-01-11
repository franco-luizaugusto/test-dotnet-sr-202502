using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicantTrackingDbContext _dbContext;

    public UnitOfWork(
        ApplicantTrackingDbContext dbContext,
        ICandidateRepository candidates,
        ITimelineRepository timelines)
    {
        _dbContext = dbContext;
        Candidates = candidates;
        Timelines = timelines;
    }

    public ICandidateRepository Candidates { get; }
    public ITimelineRepository Timelines { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransaction(tx);
    }
}

