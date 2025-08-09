using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Service.Services.Rest.Films.Features.DeleteFilmDescription;

public record DeleteFilmDescriptionRequest : BaseRequest.RequiringResult
{
    public Guid FilmId { get; set; }
}