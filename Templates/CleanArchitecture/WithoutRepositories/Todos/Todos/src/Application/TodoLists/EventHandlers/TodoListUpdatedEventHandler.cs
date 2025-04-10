using Todos.Application.TodoLists.TemplateModels;
using Todos.Application.Users.DTOs;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.Events;

namespace Todos.Application.TodoLists.EventHandlers;

public class TodoListUpdatedEventHandler(
    IEmailService emailService,
    IApplicationUserManager applicationUserManager,
    ITemplateService templateManager
) : INotificationHandler<TodoListUpdatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IApplicationUserManager _applicationUserManager = applicationUserManager;
    private readonly ITemplateService _templateManager = templateManager;

    public async Task Handle(TodoListUpdatedEvent notification, CancellationToken cancellationToken)
    {
        TodoListEntity todoList = notification.TodoList;

        UserDto user = await _applicationUserManager.GetUserByIdAsync(todoList.CreatedBy);

        string emailBody = await _templateManager.GetRenderedTemplateAsync(
            new TodoListUpdatedTemplate(
                user.UserName,
                todoList.Title,
                todoList.LastModified.ToString("g")
            )
        );

        await _emailService.SendEmailAsync(
            to: user.UserName,
            address: user.Email,
            subject: $"Todo list \"{todoList.Title}\" updated",
            body: emailBody,
            isHtml: true
        );
    }
}
