using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Services.Rest.Films.Features.GetFilmDescriptions;

public class GetFilmDescriptionsRequestHandler : BaseHandler.WithResult<IEnumerable<FilmDescription>>.ForRequest<GetFilmDescriptionsRequest>
{
    protected override async Task<OperationResult<IEnumerable<FilmDescription>>> HandleAsync(
        GetFilmDescriptionsRequest request, CancellationToken cancellationToken)
    {
        var descriptions = new FilmDescription[]
        {
            new() {
                Description = "Nice film",
                DescriptionId = Guid.NewGuid(),
                FilmId = Guid.NewGuid(),
                ReleaseDate = DateTime.Now,
                Title = "A film"
            }
        };

        return Ok(descriptions);
    }
}
