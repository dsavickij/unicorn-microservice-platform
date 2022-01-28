using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.eShop.CartService.Controllers;

public record RemoveItemRequest : IRequest<OperationResult>
{
    public Guid ItemId { get; set; }
}