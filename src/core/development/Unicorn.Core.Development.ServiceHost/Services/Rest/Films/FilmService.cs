using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.GetFilmDescription;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UpdateFilmDescription;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films;

public class FilmService : VerticallySlicedService, IServiceHostService
{
    public FilmService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    [EndpointSummary("DeleteFilmDescriptionAsync")]
    public async Task<OperationResult> DeleteFilmDescriptionAsync(Guid id)
    {
        return await SendAsync(new DeleteFilmDescriptionRequest { FilmId = id });
    }

    [EndpointSummary("GetFilmDescriptionAsync")]
    [AllowAnonymous]
    public async Task<OperationResult<FilmDescription>> GetFilmDescriptionAsync(Guid id)
    {
        return await SendAsync(new GetFilmDescriptionRequest { FilmId = id });
    }

    [EndpointSummary("UpdateFilmDescription-new")]
    public async Task<OperationResult<FileUploadResult>> UploadFilmAsyncNew([FromForm] SDK.Services.Rest.UploadDto dto,
        IFormFile file)
    {
        // Validate DTO
        if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Email))
        {
            return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
                "DTO contains invalid or missing fields."));
        }

        // Process files (e.g., save to disk)
        var allowedExtensions = new[] { ".txt", ".pdf", ".jpg", ".png" };
        var maxFileSize = 10 * 1024 * 1024; // 10MB

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
                $"Invalid file extension: {file.FileName}"));
        }

        if (file.Length > maxFileSize)
        {
            return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
                $"File {file.FileName} exceeds size limit."));
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads",
            Path.GetFileName(file.FileName));
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        // Return response

        return OperationResults.Ok(new FileUploadResult(Guid.NewGuid()));
    }

    [EndpointSummary("UpdateFilmDescription")]
    public async Task<OperationResult<FilmDescription>> UpdateFilmDescription(FilmDescription description)
    {
        return await SendAsync(new UpdateFilmDescriptionRequest { NewDescription = description });
    }

    [EndpointSummary("UploadFilmAsync")]
    public Task<OperationResult<int>> UploadFilmAsync(IFormFile File)
    {
        return Task.FromResult(new OperationResult<int>(OperationStatusCode.Status200OK, 1));
    }
}