using ApplicantTracking.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Abstractions;

public interface ITimelineRepository : IRepository<Timeline>
{
    Task<List<Timeline>> ListByAggregateRootAsync(int idAggregateRoot, CancellationToken cancellationToken);
}

