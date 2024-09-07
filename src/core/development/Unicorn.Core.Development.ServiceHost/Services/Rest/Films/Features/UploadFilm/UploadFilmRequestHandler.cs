using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UploadFilm;

public class UploadFilmRequestHandler : BaseHandler.WithResult<Guid>.ForRequest<UploadFilmRequest>
{
    protected override async Task<OperationResult<Guid>> HandleAsync(UploadFilmRequest request, CancellationToken cancellationToken)
    {
        return Ok(Guid.NewGuid());
    }
}
