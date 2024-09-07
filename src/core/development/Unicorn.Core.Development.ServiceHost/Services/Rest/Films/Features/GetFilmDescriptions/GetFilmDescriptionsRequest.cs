using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.GetFilmDescriptions;

public record GetFilmDescriptionsRequest : BaseRequest.RequiringResult<IEnumerable<FilmDescription>>
{
    public int Quantity { get; set; }
}