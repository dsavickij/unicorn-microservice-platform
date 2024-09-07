using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IServiceHostServiceRefit
{
    [Delete("/api/films/{id}/description")]
    Task<OperationResult> DeleteFilmDescriptionAsync(Guid id);

    [Get("/api/films/{id}/description")]
    Task<OperationResult<FilmDescription>> GetFilmDescriptionAsync(Guid id);

    [Post("/api/films/upload")]
    Task<OperationResult<int>> UploadFilmAsync(IFormFile File);

    [Put("/api/films/description")]
    Task<OperationResult<FilmDescription>> UpdateFilmDescription(FilmDescription description);

    //[UnicornOneWay]
    //Task SendMessageOneWay(int number);

    //[UnicornOneWay]
    //Task SendMessageOneWay2();
}