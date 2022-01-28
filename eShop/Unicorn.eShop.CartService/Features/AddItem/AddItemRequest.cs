using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

public record AddItemRequest : IRequest<OperationResult>
{
    public CartItemDTO Item { get; set; } = new CartItemDTO();
}