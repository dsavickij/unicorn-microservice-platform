using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Service.Services.Rest.Films.Features.GetFilmDescriptions;

public record GetFilmDescriptionsRequest : BaseRequest.RequiringResult<IEnumerable<FilmDescription>>
{
    public int Quantity { get; set; }
}