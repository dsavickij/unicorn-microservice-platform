using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.GetFilmDescription;

public class GetFilmDescriptionRequestHandler : BaseHandler.WithResult<FilmDescription>.ForRequest<GetFilmDescriptionRequest>
{
    protected override async Task<OperationResult<FilmDescription>> HandleAsync(
        GetFilmDescriptionRequest request, CancellationToken cancellationToken)
    {
        var description = new FilmDescription
        {
            Description = "Nice film",
            DescriptionId = Guid.NewGuid(),
            FilmId = request.FilmId,
            ReleaseDate = DateTime.Now,
            Title = "A film"
        };

        return Ok(description);
    }
}
