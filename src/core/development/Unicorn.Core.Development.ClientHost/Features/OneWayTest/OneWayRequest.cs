using MediatR;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ClientHost.Features.OneWayTest;

public record OneWayRequest : BaseRequest.RequiringNoResult
{
    public int MyProperty { get; set; }
}
