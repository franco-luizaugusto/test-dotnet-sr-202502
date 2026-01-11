using ApplicantTracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Infrastructure.Repositories;

public class Repository<TEntity> : ApplicantTracking.Application.Abstractions.IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicantTrackingDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(ApplicantTrackingDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query() => DbSet.AsQueryable();

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
        DbSet.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void Remove(TEntity entity) => DbSet.Remove(entity);
}

