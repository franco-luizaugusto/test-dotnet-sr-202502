using ApplicantTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.Data;

public sealed class ApplicantTrackingDbContext : DbContext
{
    public ApplicantTrackingDbContext(DbContextOptions<ApplicantTrackingDbContext> options) : base(options) { }

    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Timeline> Timelines => Set<Timeline>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicantTrackingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

