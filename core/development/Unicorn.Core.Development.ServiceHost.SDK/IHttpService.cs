using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Attributes;

[assembly: UnicornAssemblyServiceName("Development.HttpService")]

namespace Unicorn.Core.Development.ServiceHost.SDK;

[UnicornHttpServiceMarker]
public interface IHttpService
{
    //  [UnicornHttpGet("api/films")]
    //  Task<OperationResult<IEnumerable<FilmDescriptionDTO>>> GetFilmDescriptionsAsync(int quantity = 3);

    //  [UnicornHttpGet("api/films/{title}")]
    //  Task<OperationResult<FilmDescriptionDTO>> GetFilmDescriptionAsync(Guid id);

    //  [UnicornHttpPost("films/description/upload")]
    //  public Task<OperationResult<int>> UploadFilmAsync(IFormFile file, bool isManualReviewRequired);

    // [UnicornHttpPost("Uploadfile2/{txt}")]
    // Task<OperationResult<int>> CreateFilmDescription(string txt, [UnicornFromBody] string second);

    // fromBody cannot be used with IfOrm file, unsupported media type error
    // [UnicornHttpPost("Uploadfile/{txt}")]
    // Task<int> UploadFileAsync(string txt, [UnicornFromBody] string second, IFormFile file);

    [UnicornOneWay]
    Task SendMessageOneWay(int number);

    [UnicornOneWay]
    Task SendMessageOneWay2();
}
