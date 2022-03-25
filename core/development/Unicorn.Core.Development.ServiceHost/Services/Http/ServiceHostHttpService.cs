using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;
using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescriptions;
using Unicorn.Core.Development.ServiceHost.Features.UpdateFilmDescription;
using Unicorn.Core.Development.ServiceHost.Features.UploadFilm;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Http;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;

namespace Unicorn.Core.Development.ServiceHost.Services.Http;

public class ServiceHostHttpService : UnicornHttpService<IServiceHostService>, IServiceHostService
{
    private readonly ILogger<ServiceHostHttpService> _logger;

    public ServiceHostHttpService(ILogger<ServiceHostHttpService> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/films/descriptions")]
    public async Task<OperationResult<IEnumerable<FilmDescription>>> GetFilmDescriptionsAsync(int quantity = 3)
    {
        return await SendAsync(new GetFilmDescriptionsRequest { Quantity = Math.Clamp(quantity, 0, 100) });
    }

    [HttpGet("api/films/{id}/description")]
    public async Task<OperationResult<FilmDescription>> GetFilmDescriptionAsync(Guid id)
    {
        return await SendAsync(new GetFilmDescriptionRequest { FilmId = id });
    }

    [HttpPost("api/films/upload")]
    public async Task<OperationResult<int>> UploadFilmAsync(IFormFile file, [FromBody] FilmDescription description)
    {
        var ms = new MemoryStream();
        file.CopyTo(ms);

        return await SendAsync(new UploadFilmRequest { Film = file, Description = description });
    }

    [HttpPut("api/films/description")]
    public async Task<OperationResult<FilmDescription>> UpdateFilmDescription([FromBody] FilmDescription description)
    {
        return await SendAsync(new UpdateFilmDescriptionRequest { NewDescription = description });
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
