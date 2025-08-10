using Unicorn.Core.Development.Service.SDK.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Server.Services.Rest.Films.Features.UploadFilm;

public record UploadFilmRequest : BaseRequest.RequiringResult<Guid>
{
    public IFormFile Film { get; set; }
    public FilmDescription Description { get; set; } = new();
}