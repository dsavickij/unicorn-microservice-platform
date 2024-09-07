//using System.Reflection;
//using Castle.DynamicProxy;
//using Microsoft.Extensions.Logging;
//using Unicorn.Core.Infrastructure.Communication.MessageBroker.Queue;
//using Unicorn.Core.Infrastructure.Communication.MessageBroker.Queue.Message;

//namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy;

//internal class HttpServiceInvocationInterceptor : IInterceptor
//{
//    private readonly ILogger _logger;
//    private readonly Type _taskType = typeof(Task);
//    private readonly IHttpRequestDispatcher _httpRequestDispatcher;
//    private readonly IQueueMessageDispatcher _queueMessageDispatcher;

//    public HttpServiceInvocationInterceptor(
//        IHttpRequestDispatcher httpRequestDispatcher,
//        IQueueMessageDispatcher queueMessageDispatcher,
//        ILogger<HttpServiceInvocationInterceptor> logger)
//    {
//        _httpRequestDispatcher = httpRequestDispatcher;
//        _queueMessageDispatcher = queueMessageDispatcher;
//        _logger = logger;
//    }

//    public void Intercept(IInvocation invocation)
//    {
//        if (invocation.Method.ReturnType.BaseType == _taskType)
//        {
//            ExecuteHttpRequestDispatchAsync(invocation);
//        }
//        else
//        {
//            invocation.ReturnValue = invocation.ReturnValue switch
//            {
//                _ when invocation.Method.ReturnType == _taskType && IsOneWayMethod(invocation.Method) =>
//                    ExecuteOneWayMessageDispatchAsync(invocation),
//                _ when invocation.Method.ReturnType == _taskType =>
//                    ExecuteNoResultHttpRequestDispatchAsync(invocation)
//            };
//        }
//    }

//    private async Task ExecuteOneWayMessageDispatchAsync(IInvocation invocation)
//    {
//        var msg = new UnicornQueueMessage
//        {
//            MethodName = invocation.Method.Name,
//            Arguments = invocation.Arguments.Select(arg => new Argument
//            {
//                TypeName = arg.GetType().AssemblyQualifiedName!,
//                Value = arg
//            })
//        };

//        var queueName = QueueNameFormatter.GetNamespaceBasedName(invocation.Method);

//        await _queueMessageDispatcher.SendAsync(queueName, msg);
//    }

//    private async Task ExecuteNoResultHttpRequestDispatchAsync(IInvocation invocation) =>
//        await _httpRequestDispatcher.SendNoResultHttpRequestAsync(invocation.Method, invocation.Arguments);

//    private void ExecuteHttpRequestDispatchAsync(IInvocation invocation)
//    {
//        var returnType = invocation.Method.ReturnType;

//        var tcsType = typeof(TaskCompletionSource<>).MakeGenericType(returnType.GetGenericArguments()[0]);
//        var tcs = Activator.CreateInstance(tcsType);
//        invocation.ReturnValue = tcsType.GetProperty("Task")!.GetValue(tcs, null);

//        ExecuteGenericTaskReturnTypeInvocationAsync(invocation).ContinueWith(_ =>
//        {
//            tcsType.GetMethod("SetResult")!.Invoke(tcs, new object[] { invocation.ReturnValue! });
//        });
//    }

//    private bool IsOneWayMethod(MethodInfo method) =>
//        method.GetCustomAttributes(true).FirstOrDefault(x => x is UnicornOneWayAttribute) is not null;

//    private async Task ExecuteGenericTaskReturnTypeInvocationAsync(IInvocation invocation) =>
//        invocation.ReturnValue = await _httpRequestDispatcher.SendHttpRequestAsync(invocation.Method, invocation.Arguments);
//}
