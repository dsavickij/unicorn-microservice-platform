using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescriptions;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmsDescription;
using Unicorn.Core.Development.ServiceHost.Features.UpdateFilmDescription;
using Unicorn.Core.Development.ServiceHost.Features.UploadFilm;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

public class HttpServiceController : UnicornHttpService<IHttpService>, IHttpService
{
    private readonly ILogger<HttpServiceController> _logger;

    public HttpServiceController(ILogger<HttpServiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/films/descriptions")]
    public async Task<OperationResult<IEnumerable<FilmDescriptionDTO>>> GetFilmDescriptionsAsync(int quantity = 3)
    {
        return await Mediator.Send(new GetFilmDescriptionsRequest { Quantity = Math.Clamp(quantity, 0, 100) });
    }

    [HttpGet("api/films/{id}/description")]
    public async Task<OperationResult<FilmDescriptionDTO>> GetFilmsDescriptionAsync(Guid id)
    {
        return await Mediator.Send(new GetFilmsDescriptionRequest { FilmId = id });
    }

    [HttpPost("api/films/upload")]
    public async Task<OperationResult<int>> UploadFilmAsync(IFormFile file, [FromBody] FilmDescriptionDTO description)
    {
        var ms = new MemoryStream();
        file.CopyTo(ms);

        return await Mediator.Send(new UploadFilmRequest { Film = file, Description = description });
    }

    [HttpPut("api/films/description")]
    public async Task<OperationResult<FilmDescriptionDTO>> UpdateFilmDescription([FromBody] FilmDescriptionDTO description)
    {
        return await Mediator.Send(new UpdateFilmDescriptionRequest { NewDescription = description });
    }

    [HttpGet("api/films/{id}/download")]
    public Task<OperationResult<int>> DownloadFilmAsync(Guid id)
    {
        return Task.FromResult(new OperationResult<int>(OperationStatusCode.Status200OK, 1));
    }

    [HttpDelete("api/films/{id}/all-assets")]
    public Task<OperationResult<int>> DeleteAllFilmAssetsAsync(Guid id)
    {
        return Task.FromResult(new OperationResult<int>(OperationStatusCode.Status200OK, 1));
    }

    [NonAction]
    public Task SendMessageOneWay(int number)
    {
        return Task.CompletedTask;
    }

    [NonAction]
    public Task SendMessageOneWay2()
    {
        return Task.CompletedTask;
    }
}
