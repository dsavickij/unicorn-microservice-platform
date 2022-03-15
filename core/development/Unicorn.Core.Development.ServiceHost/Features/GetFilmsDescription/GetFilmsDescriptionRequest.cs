using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmsDescription;

internal class GetFilmsDescriptionRequest : IRequest<OperationResult<FilmDescription>>
{
    public Guid FilmId { get; set; }
}