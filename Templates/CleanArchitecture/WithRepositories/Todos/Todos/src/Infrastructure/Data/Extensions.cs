using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Todos.Infrastructure.Data.Results;

namespace Todos.Infrastructure.Data;

public static class Extensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
        where TDestination : class
    {
        return PaginatedList<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            pageNumber,
            pageSize,
            cancellationToken
        );
    }

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable,
        CancellationToken cancellationToken
    )
        where TDestination : class
    {
        return queryable
            .ProjectToType<TDestination>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null
            && r.TargetEntry.Metadata.IsOwned()
            && (
                r.TargetEntry.State == EntityState.Added
                || r.TargetEntry.State == EntityState.Modified
            )
        );
    }
}
