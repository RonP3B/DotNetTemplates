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
}
