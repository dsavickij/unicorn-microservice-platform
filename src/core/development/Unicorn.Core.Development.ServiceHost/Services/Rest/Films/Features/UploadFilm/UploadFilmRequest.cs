using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UploadFilm;

public record UploadFilmRequest : BaseRequest.RequiringResult<Guid>
{
    public IFormFile Film { get; set; }
    public FilmDescription Description { get; set; } = new();
}