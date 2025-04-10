using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Todos.Application.Shared.Interfaces;
using Todos.Domain.Shared.Bases;

namespace Todos.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(ICurrentUser currentUser, TimeProvider dateTime)
    : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly TimeProvider _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        // Ensure context and authenticated user are available before proceeding with entity updates
        if (context == null || _currentUser.Id == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (!ShouldUpdateAuditProperties(entry))
            {
                continue;
            }

            DateTimeOffset utcNow = _dateTime.GetUtcNow();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUser.Id;
                entry.Entity.Created = utcNow;
            }

            entry.Entity.LastModifiedBy = _currentUser.Id;
            entry.Entity.LastModified = utcNow;
        }
    }

    private static bool ShouldUpdateAuditProperties(EntityEntry<BaseAuditableEntity> entry)
    {
        return entry.State is EntityState.Added or EntityState.Modified
            || entry.HasChangedOwnedEntities();
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null
            && r.TargetEntry.Metadata.IsOwned()
            && (
                r.TargetEntry.State == EntityState.Added
                || r.TargetEntry.State == EntityState.Modified
            )
        );
}
