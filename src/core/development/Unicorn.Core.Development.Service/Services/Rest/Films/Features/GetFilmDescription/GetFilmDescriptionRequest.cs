using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Service.Services.Rest.Films.Features.GetFilmDescription;

public record GetFilmDescriptionRequest : BaseRequest.RequiringResult<FilmDescription>
{
    public Guid FilmId { get; set; }
}