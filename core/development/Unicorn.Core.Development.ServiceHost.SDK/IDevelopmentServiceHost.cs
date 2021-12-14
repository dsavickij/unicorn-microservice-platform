﻿using Microsoft.AspNetCore.Http;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Common.Operation;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.ParameterBindings;

[assembly: UnicornAssemblyServiceName("Development.HttpService")]

namespace Unicorn.Core.Infrastructure.Development.ServiceHost.SDK;

[UnicornHttpServiceMarker]
public interface IDevelopmentHttpService
{
    [UnicornHttpGet("GetName")]
    Task<OperationResult<string>> GetNameAsync();

    [UnicornHttpGet("GetName/{name}")]
    Task<OperationResult> GetNameAsync(string name);

    [UnicornHttpPost("Uploadfile/{txt}")]
    Task<OperationResult<int>> UploadFileAsync(string txt, string second, IFormFile file);

    [UnicornHttpPost("Uploadfile2/{txt}")]
    Task<OperationResult<int>> UploadFileAsync2(string txt, [UnicornFromBody] string second);

    // fromBody cannot be used with IfOrm file, unsupported media type error
    // [UnicornHttpPost("Uploadfile/{txt}")]
    // Task<int> UploadFileAsync(string txt, [UnicornFromBody] string second, IFormFile file);
}
