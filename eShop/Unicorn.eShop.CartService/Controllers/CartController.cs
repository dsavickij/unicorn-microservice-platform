using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

// [Authorize]
public class CartController : UnicornBaseController<ICartService>, ICartService
{
    private readonly ILogger<CartController> _logger;
    private readonly CartDbContext _ctx;

    public CartController(ILogger<CartController> logger, CartDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpPost("api/add-item")]
    public async Task<OperationResult> AddItemAsync([FromBody] CartItemDTO cartItem)
    {
        return await Mediator.Send(new AddItemRequest { Item = cartItem });
    }

    [HttpGet("api/my-cart")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
        //  using var con = new NpgsqlConnection(_settings.Value.DbConnectionString);
        //  con.Open();

        //   var sql = "SELECT version()";

        //   using var cmd = new NpgsqlCommand(sql, con);

        //   var version = cmd.ExecuteScalar()!.ToString();

        //  await _ctx.Database.EnsureDeletedAsync();
        await _ctx.Database.EnsureCreatedAsync();

        await _ctx.Carts.AddAsync(new Cart { Id = Guid.NewGuid(), Name = "text" });
        await _ctx.SaveChangesAsync(); ;

        return await Mediator.Send(new GetMyCartRequest());
    }

    [HttpDelete("api/remove-item/{itemId}")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid itemId)
    {
        return await Mediator.Send(new RemoveItemRequest { ItemId = itemId });
    }
}
