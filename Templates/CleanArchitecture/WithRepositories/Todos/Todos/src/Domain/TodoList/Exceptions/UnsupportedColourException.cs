namespace Todos.Domain.TodoList.Exceptions;

public class UnsupportedColourException(string propertyName, string code)
    : BusinessRuleException(propertyName, $"Colour '{code}' is unsupported.");
