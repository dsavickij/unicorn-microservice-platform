using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.Service.Services.Rest.Films.Features.DeleteFilmDescription;
using Unicorn.Core.Development.Service.Services.Rest.Films.Features.GetFilmDescription;
using Unicorn.Core.Development.Service.Services.Rest.Films.Features.UpdateFilmDescription;
using Unicorn.Core.Development.Service.Services.Rest.Films.Features.UploadFilm;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK;

namespace Unicorn.Core.Development.Service.Services.Rest.Films;

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
    public async Task<OperationResult<Guid>> UploadFilm(
        [FromForm] FilmDescription filmDescription,
        IFormFile file)
    {
        return await SendAsync(new UploadFilmRequest { Description = filmDescription, Film = file });
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