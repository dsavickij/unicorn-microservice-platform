using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.eShop.Catalog.Features.SoftDeleteItem;

public record SoftDeleteItemRequest : BaseRequest.RequiringResult
{
    public Guid Id { get; set; }
}
