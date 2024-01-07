using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker.Abstractions;
public interface IUnicornEventHandler<T> : IConsumer<T>
    where T : class
{
}
