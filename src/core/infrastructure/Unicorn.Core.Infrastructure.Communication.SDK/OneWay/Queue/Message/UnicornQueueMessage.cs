﻿namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue.Message;

public record UnicornQueueMessage
{
    public string MethodName { get; set; } = string.Empty;
    public IEnumerable<Argument> Arguments { get; set; } = Array.Empty<Argument>();
}
