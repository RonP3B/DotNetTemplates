using System.Collections.Concurrent;
using Todos.Application.Shared.Interfaces;
using Todos.Application.Shared.Models;

namespace Todos.Infrastructure.FileServices;

public class TemplateManager : ITemplateService
{
    private readonly ConcurrentDictionary<string, string> _templateCache = new();

    private readonly string _baseTemplatesPath = Path.Combine(
        Directory.GetParent(Environment.CurrentDirectory)!.FullName,
        nameof(Infrastructure),
        "Templates"
    );

    public Task<string> GetRenderedTemplateAsync(TemplateModel model)
    {
        string templateContent = GetTemplateContent(model);

        foreach (var placeholder in model.GetPlaceholders())
        {
            templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
        }

        return Task.FromResult(templateContent);
    }

    private string GetTemplateContent(TemplateModel model)
    {
        string templateName = model.TemplateName;
        string templateFileType = model.TemplateFileType.ToString();
        string cacheKey = $"{templateName}_{templateFileType}";

        return _templateCache.GetOrAdd(
            cacheKey,
            _ =>
            {
                string templatePath = Path.Combine(
                    _baseTemplatesPath,
                    templateFileType,
                    $"{templateName}.{templateFileType.ToLower()}"
                );

                if (!File.Exists(templatePath))
                {
                    throw new FileNotFoundException(
                        $"{templateFileType} template '{templateName}' not found"
                    );
                }

                return File.ReadAllText(templatePath);
            }
        );
    }
}
