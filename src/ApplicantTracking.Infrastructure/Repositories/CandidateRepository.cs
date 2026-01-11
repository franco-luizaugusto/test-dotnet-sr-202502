using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Infrastructure.Repositories;

public sealed class CandidateRepository : Repository<Candidate>, ICandidateRepository
{
    public CandidateRepository(ApplicantTrackingDbContext dbContext) : base(dbContext) { }

    public Task<Candidate?> GetByIdAsync(int idCandidate, CancellationToken cancellationToken) =>
        DbContext.Candidates.FirstOrDefaultAsync(x => x.IdCandidate == idCandidate, cancellationToken);

    public Task<List<Candidate>> ListAsync(CancellationToken cancellationToken) =>
        DbContext.Candidates.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<(List<Candidate> Items, int TotalCount)> ListPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = DbContext.Candidates.AsNoTracking().OrderBy(x => x.IdCandidate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email)) return Task.FromResult(false);

        // Candidate.Email is normalized to lowercase on write (domain rule).
        // We also normalize here to match stored values.
        var normalized = email.Trim().ToLowerInvariant();

        return DbContext.Candidates
            .AsNoTracking()
            .AnyAsync(x => x.Email == normalized, cancellationToken);
    }
}

