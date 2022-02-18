using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.AddItem;

public record AddItemRequest : BaseRequest.WithResponse
{
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public CartItemDTO Item { get; set; } = new CartItemDTO();
}
