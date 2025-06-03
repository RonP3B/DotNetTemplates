using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.ArchiveCategory;

internal sealed class ArchiveCategoryCommandHandler(
    ICategoryRepository categories,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveCategoryCommand, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categories.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));
        
        if (category.IsArchived)
            return Result.Failure<CategoryResponse>(CategoryErrors.AlreadyArchived);
        
        var result = category.Archive();
        if (result.IsFailure)
            return Result.Failure<CategoryResponse>(result.Error);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (CategoryResponse)category;
    }
}