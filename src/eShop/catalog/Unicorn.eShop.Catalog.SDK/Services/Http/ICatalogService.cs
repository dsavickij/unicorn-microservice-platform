using Refit;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.eShop.Catalog.SDK;
using Unicorn.eShop.Catalog.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Catalog.SDK.Services.Http;

[UnicornRestServiceMarker]
public interface ICatalogService
{
    [Delete("api/catalog/items/{id}/soft")]
    Task<OperationResult> SoftDeleteItemAsync(Guid id);

    [Post("api/catalog/categories")]
    Task<OperationResult<CatalogCategory>> CreateCatalogCategoryAsync([Body] CatalogCategory catalogCategory);
}
