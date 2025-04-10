namespace Todos.Domain.Shared.Interfaces;

public interface IDomainEventEntity
{
    IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
