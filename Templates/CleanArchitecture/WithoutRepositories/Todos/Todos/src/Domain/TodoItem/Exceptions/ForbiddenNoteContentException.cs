namespace Todos.Domain.TodoItem.Exceptions;

public class ForbiddenNoteContentException(string propertyName)
    : BusinessRuleException(propertyName, "The note contains forbidden content.");
