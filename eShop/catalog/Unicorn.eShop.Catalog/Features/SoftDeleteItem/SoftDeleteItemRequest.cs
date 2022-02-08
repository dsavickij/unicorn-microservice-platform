using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.eShop.Catalog.Features.SoftDeleteItem;

public record SoftDeleteItemRequest : BaseRequest.WithResponse
{
    public Guid Id { get; set; }
}
