using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.eShop.Cart.Features.RemoveItem;

public record RemoveItemRequest : BaseRequest.RequiringResult
{
    public Guid CartId { get; set; }
    public Guid CatalogItemId { get; set; }
}
