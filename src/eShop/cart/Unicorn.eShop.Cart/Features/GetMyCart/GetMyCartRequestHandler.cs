﻿using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.DataAccess;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.GetMyCart;

public class GetMyCartRequestHandler : BaseHandler.WithResult<CartDTO>.ForRequest<GetMyCartRequest>
{
    private readonly CartDbContext _ctx;

    public GetMyCartRequestHandler(CartDbContext context) => _ctx = context;

    protected override async Task<OperationResult<CartDTO>> HandleAsync(GetMyCartRequest request, CancellationToken cancellationToken)
    {
        var result = await GetMyCartRequestAsync(request);

        return result.Match(
            notFound => NotFound(),
            success => Ok(success.Value));
    }

    private async Task<OneOf<NotFound, Success<CartDTO>>> GetMyCartRequestAsync(GetMyCartRequest request)
    {
        var req = request;
        var fakeUserId = Guid.NewGuid(); // TODO: change to current userId after authentication fix for docker

        var result = await _ctx.Carts.SingleOrDefaultAsync(x => x.UserId == fakeUserId);

        return result is null ? new NotFound() : new Success<CartDTO>(new CartDTO
        {
            CartId = result.Id,
            UserId = fakeUserId,
            Items = result.Items.Select(x => new SDK.DTOs.CartItemDTO
            {
                CatalogItemId = x.CatalogItemId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }),
            TotalPrice = result.TotalPrice,
        });
    }
}
