using MediatR;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

public record UploadFilmRequest : IRequest<OperationResult<int>>
{
    public IFormFile Film { get; set; }
    public FilmDescriptionDTO Description { get; set; }
}