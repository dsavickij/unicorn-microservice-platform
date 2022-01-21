using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

public record GetFilmDescriptionsRequest : IRequest<OperationResult<IEnumerable<FilmDescriptionDTO>>>
{
    public int Quantity { get; set; }
}