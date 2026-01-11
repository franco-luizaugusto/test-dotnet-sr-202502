using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Infrastructure.Repositories;

public sealed class TimelineRepository : Repository<Timeline>, ITimelineRepository
{
    public TimelineRepository(ApplicantTrackingDbContext dbContext) : base(dbContext) { }

    public Task<List<Timeline>> ListByAggregateRootAsync(int idAggregateRoot, CancellationToken cancellationToken) =>
        DbContext.Timelines
            .AsNoTracking()
            .Where(x => x.IdAggregateRoot == idAggregateRoot)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
}

