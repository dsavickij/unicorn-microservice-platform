using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.Catalog.SDK;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Catalog.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface ICatalogService
{
    [UnicornHttpDelete("api/catalog/items/{id}/soft")]
    Task<OperationResult> SoftDeleteItemAsync([UnicornFromRoute] Guid id);
}
