using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

public record UpdateFilmDescriptionRequest : IRequest<OperationResult<FilmDescriptionDTO>>
{
    public FilmDescriptionDTO NewDescription { get; set; } = new FilmDescriptionDTO();
}