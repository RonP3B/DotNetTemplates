using Evently.Common.Application.Caching;
using Evently.Modules.Events.Application.Categories;
using Evently.Modules.Events.Application.Categories.GetCategories;

namespace Evently.Modules.Events.Presentation.Categories;

internal class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
            {
                IReadOnlyCollection<CategoryResponse>? categoryResponses =
                    await cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>("categories");

                if (categoryResponses is not null)
                {
                    return Results.Ok(categoryResponses);
                }

                var result = await sender.Send(new GetCategoriesQuery());

                if (result.IsSuccess)
                {
                    await cacheService.SetAsync("categories", result.Value);
                }

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(GetCategories))
            .WithTags(Tags.Categories)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get Categories",
                Description = "Get all categories",
            });

    }
}