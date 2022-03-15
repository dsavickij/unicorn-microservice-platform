using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Templates.Service.SDK.DTOs;

namespace Unicorn.Templates.Service.Features.CreateItem;

public record CreateItemRequest : BaseRequest.WithResponse<Item>
{
    public Item Item { get; set; } = new ();
}
