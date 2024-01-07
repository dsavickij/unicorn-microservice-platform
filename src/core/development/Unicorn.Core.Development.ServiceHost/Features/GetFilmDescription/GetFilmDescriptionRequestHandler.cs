using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;

public class GetFilmDescriptionRequestHandler : BaseHandler.WithResult<FilmDescription>.For<GetFilmDescriptionRequest>
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
