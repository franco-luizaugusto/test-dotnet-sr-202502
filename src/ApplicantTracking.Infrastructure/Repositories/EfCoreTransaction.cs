using ApplicantTracking.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Infrastructure.Repositories;

internal sealed class EfCoreTransaction : ITransaction
{
    private readonly IDbContextTransaction _tx;

    public EfCoreTransaction(IDbContextTransaction tx) => _tx = tx;

    public Task CommitAsync(CancellationToken cancellationToken) => _tx.CommitAsync(cancellationToken);

    public ValueTask DisposeAsync() => _tx.DisposeAsync();
}

