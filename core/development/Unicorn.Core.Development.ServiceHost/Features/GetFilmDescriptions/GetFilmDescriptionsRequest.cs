using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmDescriptions;

public record GetFilmDescriptionsRequest : BaseRequest.WithResponse<IEnumerable<FilmDescription>>
{
    public int Quantity { get; set; }
}