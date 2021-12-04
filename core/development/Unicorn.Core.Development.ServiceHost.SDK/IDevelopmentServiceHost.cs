using Microsoft.AspNetCore.Http;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Common;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.ParameterAttributes;

[assembly: UnicornAssemblyServiceName("Development.HttpService")]

namespace Unicorn.Core.Infrastructure.Development.ServiceHost.SDK;

[UnicornHttpServiceMarker]
public interface IDevelopmentHttpService
{
    [UnicornHttpPost("Uploadfile/{txt}")]
    Task<int> UploadFileAsync(string txt, string second, IFormFile file);

    [UnicornHttpPost("Uploadfile2/{txt}")]
    Task<int> UploadFileAsync2(string txt, [UnicornFromBody] string second);

    // fromBody cannot be used with IfOrm file, unsupported media type error
    // [UnicornHttpPost("Uploadfile/{txt}")]
    // Task<int> UploadFileAsync(string txt, [UnicornFromBody] string second, IFormFile file);
}
