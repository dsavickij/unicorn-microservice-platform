using MediatR;

namespace Unicorn.Core.Development.ClientHost.Features.OneWayTest;

public record OneWayRequest : IRequest
{
    public int MyProperty { get; set; }
}
