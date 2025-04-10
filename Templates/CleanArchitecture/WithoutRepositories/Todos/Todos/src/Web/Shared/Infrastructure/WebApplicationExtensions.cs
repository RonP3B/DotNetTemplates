using System.Reflection;

namespace Todos.Web.Shared.Infrastructure;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        string groupName = group.GetType().Name;

        return app.MapGroup($"/api/v1/{ConvertToKebabCase(groupName)}")
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithOpenApi();
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        Type endpointGroupType = typeof(EndpointGroupBase);

        Assembly assembly = Assembly.GetExecutingAssembly();

        IEnumerable<Type> endpointGroupTypes = assembly
            .GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (Type type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }

    private static string ConvertToKebabCase(this string input)
    {
        return string.Concat(
            input.Select(
                (x, i) =>
                    i > 0 && char.IsUpper(x) ? "-" + char.ToLower(x) : char.ToLower(x).ToString()
            )
        );
    }
}
