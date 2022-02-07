using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unicorn.eShop.Cart;
using Unicorn.eShop.Cart.Entities;
using static SeedDataWorkerContstants;

internal class SeedDataWorker : IHostedService
{
    private readonly IServiceProvider _provider;

    public SeedDataWorker(IServiceProvider serviceProvider) => _provider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _provider.CreateScope();

        var ctx = scope.ServiceProvider.GetRequiredService<CartDbContext>();

        await ctx.Database.EnsureCreatedAsync();

        if (await ctx.Carts.FirstOrDefaultAsync(x => x.Id == CartId_1) is null)
        {
            await ctx.Carts.AddAsync(new CartEntity
            {
                Id = CartId_1,
                UserId = UserId_1,
                Items = new[]
                {
                    new CartItemEntity
                    {
                        Id = ItemId_1,
                        CartId = CartId_1,
                        CatalogItemId = CatalogItemId_1,
                        Quantity = 5,
                        UnitPrice = ItemPrice_1
                    }
                },
            });
        }

        if (await ctx.Carts.FirstOrDefaultAsync(x => x.Id == CartId_2) is null)
        {
            await ctx.Carts.AddAsync(new CartEntity
            {
                Id = CartId_2,
                UserId = UserId_2,
                Items = new[]
                   {
                        new CartItemEntity
                        {
                            Id = ItemId_2,
                            CartId = CartId_2,
                            CatalogItemId = CatalogItemId_2,
                            Quantity = 3,
                            UnitPrice = ItemPrice_2
                        },
                        new CartItemEntity
                        {
                            Id = ItemId_3,
                            CartId = CartId_2,
                            CatalogItemId = CatalogItemId_3,
                            Quantity = 2,
                            UnitPrice = ItemPrice_3
                        }
                    },
            });
        }

        await ctx.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
