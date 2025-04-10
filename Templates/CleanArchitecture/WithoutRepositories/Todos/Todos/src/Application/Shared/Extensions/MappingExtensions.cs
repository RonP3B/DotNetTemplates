namespace Todos.Application.Shared.Extensions;

public static class MappingExtensions
{
    public static TDestination PartialAdapt<TSource, TDestination>(
        this TSource source,
        TDestination destination
    )
        where TDestination : class
    {
        TypeAdapterConfig config = new();

        config.ForType<TSource, TDestination>().IgnoreNullValues(true);

        return source.Adapt(destination, config);
    }

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
}
