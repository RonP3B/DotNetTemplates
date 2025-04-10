namespace Todos.Domain.Shared.Bases;

public abstract class DomainException(string message) : Exception(message) { }
