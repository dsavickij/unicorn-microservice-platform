using Unicorn.Core.Development.Service.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Server.Services.Rest.Films.Features.UpdateFilmDescription;

public record UpdateFilmDescriptionRequest : BaseRequest.RequiringResult<FilmDescription>
{
    public FilmDescription NewDescription { get; set; } = new FilmDescription();
}