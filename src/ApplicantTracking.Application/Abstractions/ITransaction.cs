using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Abstractions;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
}

