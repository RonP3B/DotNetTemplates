using Microsoft.AspNetCore.Http.HttpResults;
using Todos.Application.FileManager.Commands.DeleteFile;
using Todos.Application.FileManager.Commands.UploadFile;
using Todos.Application.FileManager.Commands.UploadFiles;
using Todos.Application.FileManager.DTOs;

namespace Todos.Web.Endpoints;

public class FileManager : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .DisableAntiforgery()
            .MapPost(UploadFile)
            .MapPost(UploadFiles, "multiple")
            .MapDelete(DeleteFile, "{imageKey}");
    }

    public async Task<Ok<UploadedFileDto>> UploadFile(ISender sender, IFormFile formFile)
    {
        using Stream fileStream = formFile.OpenReadStream();

        FileDataDto fileData = new(formFile.FileName, formFile.ContentType, fileStream);

        UploadedFileDto uploadedFile = await sender.Send(new UploadFileCommand(fileData));

        return TypedResults.Ok(uploadedFile);
    }

    public async Task<Ok<IEnumerable<UploadedFileDto>>> UploadFiles(
        ISender sender,
        IFormFileCollection formFiles
    )
    {
        List<FileDataDto> fileDataList = [];

        foreach (IFormFile formFile in formFiles)
        {
            using Stream fileStream = formFile.OpenReadStream();

            fileDataList.Add(new FileDataDto(formFile.FileName, formFile.ContentType, fileStream));
        }

        IEnumerable<UploadedFileDto> uploadedFiles = await sender.Send(
            new UploadFilesCommand(fileDataList)
        );

        return TypedResults.Ok(uploadedFiles);
    }

    public async Task<NoContent> DeleteFile(string imageKey, ISender sender)
    {
        string userId = imageKey.Split('_')[0];

        await sender.Send(new DeleteFileCommand(imageKey, userId));

        return TypedResults.NoContent();
    }
}
