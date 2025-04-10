namespace Todos.Application.Shared.Interfaces;

public interface ITemplateService
{
    Task<string> GetRenderedTemplateAsync(TemplateModel model);
}
