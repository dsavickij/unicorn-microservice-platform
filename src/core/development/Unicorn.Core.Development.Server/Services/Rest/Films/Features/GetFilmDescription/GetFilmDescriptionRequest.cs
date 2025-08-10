using Unicorn.Core.Development.Service.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Server.Services.Rest.Films.Features.GetFilmDescription;

public record GetFilmDescriptionRequest : BaseRequest.RequiringResult<FilmDescription>
{
    public Guid FilmId { get; set; }
}