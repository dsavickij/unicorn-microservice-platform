using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;

[assembly:UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;

[UnicornRestServiceMarker]
public interface IServiceHostService
{
    [Delete("/api/films/{id}/description")]
    Task<OperationResult> DeleteFilmDescriptionAsync(Guid id);

    [Get("/api/films/{id}/description")]
    Task<OperationResult<FilmDescription>> GetFilmDescriptionAsync(Guid id);

    [Post("/api/films/upload")]
    Task<OperationResult<int>> UploadFilmAsync(IFormFile file);
    
    [Post("/api/films/upload-new")]
    Task<OperationResult<Guid>> UploadFilm([FromForm] FilmDescription filmDescription, [Body] IFormFile file);

    [Put("/api/films/description")]
    Task<OperationResult<FilmDescription>> UpdateFilmDescription(FilmDescription description);

    //[UnicornOneWay]
    //Task SendMessageOneWay(int number);

    //[UnicornOneWay]
    //Task SendMessageOneWay2();
}