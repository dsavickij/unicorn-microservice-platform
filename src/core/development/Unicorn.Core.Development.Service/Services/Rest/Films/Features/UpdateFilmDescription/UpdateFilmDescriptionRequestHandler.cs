using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Service.Services.Rest.Films.Features.UpdateFilmDescription;

public class UpdateFilmDescriptionsRequestHandler : BaseHandler.WithResult<FilmDescription>.ForRequest<UpdateFilmDescriptionRequest>
{
    protected override async Task<OperationResult<FilmDescription>> HandleAsync(
        UpdateFilmDescriptionRequest request, CancellationToken cancellationToken)
    {
        var updateDescription = request.NewDescription with { LastUpdatedOn = DateTime.UtcNow };

        return Ok(updateDescription);
    }
}
