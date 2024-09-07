using Microsoft.AspNetCore.Authorization;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Http;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.GetFilmDescription;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UpdateFilmDescription;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films;

public class FilmService : VerticallySlicedService, IServiceHostServiceRefit
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