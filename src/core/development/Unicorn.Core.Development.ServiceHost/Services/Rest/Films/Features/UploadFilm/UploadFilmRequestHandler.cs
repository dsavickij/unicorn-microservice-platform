using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UploadFilm;

public class UploadFilmRequestHandler : BaseHandler.WithResult<Guid>.ForRequest<UploadFilmRequest>
{
    protected override async Task<OperationResult<Guid>> HandleAsync(UploadFilmRequest request, CancellationToken cancellationToken)
    {
        // TODO: move to validation
        // Validate DTO
        // if (string.IsNullOrEmpty(filmDescription) || string.IsNullOrEmpty(filmDescription.Email))
        // {
        //     return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
        //         "DTO contains invalid or missing fields."));
        // }

        // Process files (e.g., save to disk)
        var allowedExtensions = new[] { ".txt", ".pdf", ".jpg", ".png" };
        var maxFileSize = 10 * 1024 * 1024; // 10MB

        var extension = Path.GetExtension(request.Film.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
                $"Invalid file extension: {request.Film.FileName}"));
        }

        if (request.Film.Length > maxFileSize)
        {
            return OperationResults.BadRequest(new OperationError(OperationStatusCode.Status400BadRequest,
                $"File {request.Film.FileName} exceeds size limit."));
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads",
            Path.GetFileName(request.Film.FileName));
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await request.Film.CopyToAsync(stream);

        // Return response
        return Ok(Guid.NewGuid());
    }
}
