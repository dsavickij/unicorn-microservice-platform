using MassTransit;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Abstractions;

public interface IUnicornEventHandler<T> : IConsumer<T>
    where T : class
{
}
