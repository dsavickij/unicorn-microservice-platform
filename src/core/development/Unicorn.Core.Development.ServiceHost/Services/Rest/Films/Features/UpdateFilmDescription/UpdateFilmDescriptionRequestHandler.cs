using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.UpdateFilmDescription;

public class UpdateFilmDescriptionsRequestHandler : BaseHandler.WithResult<FilmDescription>.ForRequest<UpdateFilmDescriptionRequest>
{
    protected override async Task<OperationResult<FilmDescription>> HandleAsync(
        UpdateFilmDescriptionRequest request, CancellationToken cancellationToken)
    {
        var updateDescription = request.NewDescription with { LastUpdatedOn = DateTime.UtcNow };

        return Ok(updateDescription);
    }
}
