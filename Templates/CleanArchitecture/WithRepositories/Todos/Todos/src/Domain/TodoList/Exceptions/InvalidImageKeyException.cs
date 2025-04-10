namespace Todos.Domain.TodoList.Exceptions;

public class InvalidImageKeyException(string propertyName, string fileName)
    : BusinessRuleException(
        propertyName,
        $"The image key '{fileName}' does not meet the required format. "
            + "Expected format: {UUID}_{UUID}_{ImageName.Extension}"
    );
