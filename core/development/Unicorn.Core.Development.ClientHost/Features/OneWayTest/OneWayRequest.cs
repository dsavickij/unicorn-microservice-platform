using MediatR;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ClientHost.Features.OneWayTest;

public record OneWayRequest : BaseRequest.WithoutResponse
{
    public int MyProperty { get; set; }
}
