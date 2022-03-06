using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.Templates.Service.SDK;
using Unicorn.Templates.Service.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Templates.Service.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IYOUR_SERVICE_NAMEService
{
    [UnicornHttpPost("api/YOUR_SERVICE_NAME/items")]
    Task<OperationResult<Item>> CreateItemAsync([UnicornFromBody] Item item);

    [UnicornHttpPut("api/YOUR_SERVICE_NAME/items")]
    Task<OperationResult<Item>> UpdateItemAsync([UnicornFromBody] Item item);

    [UnicornHttpGet("api/YOUR_SERVICE_NAME/items/{id}")]
    Task<OperationResult<Item>> GetItemAsync([UnicornFromRoute] Guid id);

    [UnicornHttpDelete("api/YOUR_SERVICE_NAME/items/{id}")]
    Task<OperationResult> DeleteItemAsync([UnicornFromRoute] Guid id);

    [UnicornOneWay]
    Task UpdateAllItemPricesAsync();
}
