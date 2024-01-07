using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Development.ServiceHost.Features.UpdateFilmDescription;

public record UpdateFilmDescriptionRequest : IRequest<OperationResult<FilmDescription>>
{
    public FilmDescription NewDescription { get; set; } = new FilmDescription();
}