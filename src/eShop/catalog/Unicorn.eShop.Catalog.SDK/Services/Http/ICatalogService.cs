using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.Catalog.SDK;
using Unicorn.eShop.Catalog.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Catalog.SDK.Services.Http;

[UnicornRestServiceMarker]
public interface ICatalogService
{
    [UnicornHttpDelete("api/catalog/items/{id}/soft")]
    Task<OperationResult> SoftDeleteItemAsync([UnicornFromRoute] Guid id);

    [UnicornHttpPost("api/catalog/categories")]
    Task<OperationResult<CatalogCategory>> CreateCatalogCategoryAsync([UnicornFromBody] CatalogCategory catalogCategory);
}
