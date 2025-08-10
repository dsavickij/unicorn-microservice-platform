using Unicorn.Core.Development.Service.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Server.Services.Rest.Films.Features.GetFilmDescriptions;

public record GetFilmDescriptionsRequest : BaseRequest.RequiringResult<IEnumerable<FilmDescription>>
{
    public int Quantity { get; set; }
}