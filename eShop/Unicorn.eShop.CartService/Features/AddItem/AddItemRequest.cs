using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

public record AddItemRequest : BaseRequest.WithResponse
{
    public Guid UserId { get; set; }
    public CartItemDTO Item { get; set; } = new CartItemDTO();
}