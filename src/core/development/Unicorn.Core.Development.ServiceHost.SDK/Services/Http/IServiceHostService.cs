using Microsoft.AspNetCore.Http;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IServiceHostService
{
    [UnicornHttpGet("api/films/descriptions")]
    Task<OperationResult<IEnumerable<FilmDescription>>> GetFilmDescriptionsAsync(int quantity = 3);

    [UnicornHttpGet("api/films/{id}/description")]
    Task<OperationResult<FilmDescription>> GetFilmDescriptionAsync(Guid id);

    [UnicornHttpPost("api/films/upload")]
    Task<OperationResult<int>> UploadFilmAsync(IFormFile file, [UnicornFromBody] FilmDescription description);

    [UnicornHttpPut("api/films/description")]
    Task<OperationResult<FilmDescription>> UpdateFilmDescription([UnicornFromBody] FilmDescription description);

    [UnicornOneWay]
    Task SendMessageOneWay(int number);

    [UnicornOneWay]
    Task SendMessageOneWay2();
}
