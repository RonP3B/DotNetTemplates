namespace Todos.Domain.TodoItem.Exceptions;

public class NoteTooLongException(string propertyName, int maxLength)
    : BusinessRuleException(
        propertyName,
        $"The note exceeds the maximum allowed length of {maxLength} characters."
    );
