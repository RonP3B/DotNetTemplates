namespace Todos.Domain.Shared.Exceptions;

public class BusinessRuleException(string propertyName, string message) : DomainException(message)
{
    public string PropertyName { get; } = propertyName;
}
