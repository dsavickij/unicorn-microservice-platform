using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.eShop.Cart.DataAccess;
using Unicorn.eShop.Cart.Features.AddItem;
using Xunit;

namespace Unicorn.eShop.Cart.Tests.Features.AddItem;

internal static class InMemoryCartDbContext
{
    public static CartDbContext GetInstance()
    {
        var builder = new DbContextOptionsBuilder<CartDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new CartDbContext(builder.Options);
    }
}

public class AddItemRequestHandlerTests
{
    private readonly Fixture _fixture;

    public AddItemRequestHandlerTests() => _fixture = new Fixture();

    [Fact]
    public async Task When_CartDoesNotExist_Then_CartIsCreatedAndItemIsAdded()
    {
        // Arrange
        var request = _fixture.Create<AddItemRequest>();

        using var ctx = InMemoryCartDbContext.GetInstance();

        var sut = new AddItemRequestHandler(ctx);

        // Act
        var response = await sut.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(OperationStatusCode.Status200OK);

        await ctx.Carts.Awaiting(x => x.SingleOrDefaultAsync()).Should().NotThrowAsync();

        var result = await ctx.CartItems.Awaiting(x => x.SingleAsync()).Should().NotThrowAsync();

        result.Which.Id.Should().NotBeEmpty();
        result.Which.CartId.Should().Be(request.CartId);
        result.Which.CatalogItemId.Should().Be(request.Item.CatalogItemId);
        result.Which.Quantity.Should().Be(request.Item.Quantity);
        result.Which.UnitPrice.Should().Be(request.Item.UnitPrice);
    }

    [Fact]
    public async Task When_CartExists_Then_ItemIsAddedWithoutNewCartCreation()
    {
        // Arrange
        var request = _fixture.Create<AddItemRequest>();

        using var ctx = InMemoryCartDbContext.GetInstance();

        await ctx.Carts.AddAsync(new DataAccess.Entities.CartEntity { Id = request.CartId });
        await ctx.SaveChangesAsync();

        var sut = new AddItemRequestHandler(ctx);

        // Act
        var response = await sut.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(OperationStatusCode.Status200OK);

        var result = await ctx.CartItems.Awaiting(x => x.SingleAsync()).Should().NotThrowAsync();

        result.Which.Id.Should().NotBeEmpty();
        result.Which.CartId.Should().Be(request.CartId);
        result.Which.CatalogItemId.Should().Be(request.Item.CatalogItemId);
        result.Which.Quantity.Should().Be(request.Item.Quantity);
        result.Which.UnitPrice.Should().Be(request.Item.UnitPrice);
    }
}
