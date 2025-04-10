using Microsoft.EntityFrameworkCore;
using Todos.Domain.Shared.Bases;
using Todos.Infrastructure.Data.Contexts;

namespace Todos.Infrastructure.Data.Repositories;

public abstract class BaseRepository<TEntity, TEntityId>(ApplicationDbContext context)
    where TEntity : BaseEntity<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext _context = context;

    public virtual Task<TEntity?> GetAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual Task<TEntity?> GetForReadOnlyAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
