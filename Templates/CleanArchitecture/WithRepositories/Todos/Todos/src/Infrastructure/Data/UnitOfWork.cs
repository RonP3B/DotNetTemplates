using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Todos.Application.Shared.Interfaces;
using Todos.Domain.Shared.Bases;
using Todos.Domain.Shared.Interfaces;
using Todos.Infrastructure.Data.Contexts;

namespace Todos.Infrastructure.Data;

public class UnitOfWork(
    ApplicationDbContext context,
    TimeProvider dateTime,
    IMediator mediator,
    ICurrentUser currentUser
) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly TimeProvider _dateTime = dateTime;
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntities(_context);

        await DispatchDomainEvents(_context);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        List<IDomainEventEntity> domainEntities =
        [
            .. context
                .ChangeTracker.Entries()
                .Where(e => e.Entity is IDomainEventEntity entity && entity.DomainEvents.Count != 0)
                .Select(e => (IDomainEventEntity)e.Entity),
        ];

        List<BaseEvent> domainEvents = [.. domainEntities.SelectMany(e => e.DomainEvents)];

        domainEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null || _currentUser.Id == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
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

    private static bool ShouldUpdateAuditProperties(EntityEntry<IAuditableEntity> entry)
    {
        return entry.State is EntityState.Added or EntityState.Modified
            || entry.HasChangedOwnedEntities();
    }
}
