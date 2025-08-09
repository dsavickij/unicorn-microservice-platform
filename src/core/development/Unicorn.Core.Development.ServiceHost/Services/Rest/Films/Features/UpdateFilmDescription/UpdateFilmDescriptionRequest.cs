using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UpdateFilmDescription;

public record UpdateFilmDescriptionRequest : BaseRequest.RequiringResult<FilmDescription>
{
    public FilmDescription NewDescription { get; set; } = new FilmDescription();
}