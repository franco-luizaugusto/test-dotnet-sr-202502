using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query();
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}

