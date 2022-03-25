using Unicorn.Core.Development.ServiceHost.Features.GetFilmDescriptions;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;

public class GetFilmDescriptionsRequestHandler : BaseHandler.WithResult<IEnumerable<FilmDescription>>.For<GetFilmDescriptionsRequest>
{
    protected override async Task<OperationResult<IEnumerable<FilmDescription>>> HandleAsync(
        GetFilmDescriptionsRequest request, CancellationToken cancellationToken)
    {
        var descriptions = new FilmDescription[]
        {
            new FilmDescription
            {
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
