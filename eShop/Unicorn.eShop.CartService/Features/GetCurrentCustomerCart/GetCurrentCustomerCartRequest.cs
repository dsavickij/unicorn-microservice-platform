using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.eShop.CartService.Controllers;

public record GetMyCartRequest : IRequest<OperationResult<CartDTO>>
{
}