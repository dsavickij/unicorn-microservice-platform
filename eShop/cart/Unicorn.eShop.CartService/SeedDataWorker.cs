using Microsoft.EntityFrameworkCore;
using Unicorn.eShop.CartService;
using Unicorn.eShop.CartService.Entities;
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
            await ctx.Carts.AddAsync(new Cart
            {
                Id = CartId_1,
                UserId = UserId_1,
                Items = new[]
                {
                    new CartItem
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
            await ctx.Carts.AddAsync(new Cart
            {
                Id = CartId_2,
                UserId = UserId_2,
                Items = new[]
                   {
                        new CartItem
                        {
                            Id = ItemId_2,
                            CartId = CartId_2,
                            CatalogItemId = CatalogItemId_2,
                            Quantity = 3,
                            UnitPrice = ItemPrice_2
                        },
                        new CartItem
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

internal static class SeedDataWorkerContstants
{
    public static Guid CartId_1 = new("6f3347a2-16c2-4089-9d47-4cf1113f91e1");
    public static Guid CartId_2 = new("2c6e01a3-694b-4795-a8d5-277e0edac459");

    public static Guid CatalogItemId_1 = new("a0c85ad5-e574-4aee-bc25-c8eaf597fdc3");
    public static Guid CatalogItemId_2 = new("1af58645-5d8b-47e3-87f3-2fae21589692");
    public static Guid CatalogItemId_3 = new("8743a89e-b0d4-4f18-9e40-ce066826d5a9");

    public static Guid ItemId_1 = new("453edc2a-3ca6-4bd3-bd4e-e5f51a0090cf");
    public static Guid ItemId_2 = new("829657c2-8654-4b45-aa6d-dabd9f280240");
    public static Guid ItemId_3 = new("dadca4b8-ed03-42e3-b2ca-611871e6b535");

    public static decimal ItemPrice_1 = 1.1m;
    public static decimal ItemPrice_2 = 2.35m;
    public static decimal ItemPrice_3 = 3.49m;

    public static Guid UserId_1 = new("187d8487-7404-40c2-8aa3-39d4061ef91a");
    public static Guid UserId_2 = new("c109cdac-29d3-4d4c-bc59-4202ad3e1f34");
}