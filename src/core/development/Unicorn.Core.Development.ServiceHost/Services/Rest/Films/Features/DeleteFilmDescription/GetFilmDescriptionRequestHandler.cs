using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;

public class DeleteFilmDescriptionRequestHandler : BaseHandler.WithResult.ForRequest<DeleteFilmDescriptionRequest>
{
    protected override async Task<OperationResult> HandleAsync(DeleteFilmDescriptionRequest request, CancellationToken cancellationToken)
    {
        return Ok();
    }
}