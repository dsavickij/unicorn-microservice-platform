using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;
using Unicorn.Core.Development.ClientHost.Features.OneWayTest;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Http;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Services.Http;

public interface IClientHostService
{
}

public class ClientHostService : UnicornHttpService<IClientHostService>, IClientHostService
{
    private readonly ILogger<ClientHostService> _logger;
    private readonly IBus _bus;
    //private readonly IUnicornEventPublisher _publisher;
    private readonly IMultiplicationGrpcServiceClient _multiplicationGrpcSvcClient;
    private readonly IDivisionGrpcServiceClient _divisionGrpcSvcClient;
    private readonly ISubtractionGrpcServiceClient _subtractionGrpcSvcClient;
    private readonly IServiceDiscoveryService _svcDiscoveryService;
    private readonly IServiceHostService _developmentServiceHost;
    private readonly IServiceHostServiceRefit _refitService;
//    private readonly IAuthenticationScope _scopeProvider;

    public ClientHostService(
        IServiceDiscoveryService serviceDiscoveryService,
        IMultiplicationGrpcServiceClient multiplicationGrpcServiceClient,
        IDivisionGrpcServiceClient divisionGrpcServiceClient,
        ISubtractionGrpcServiceClient subtractionGrpcServiceClient,
        IServiceHostService developmentServiceHost,
        IServiceHostServiceRefit refitService,
     //   IAuthenticationScope scopeProvider,
        ILogger<ClientHostService> logger
        //IUnicornEventPublisher publisher
        )
    {
        _multiplicationGrpcSvcClient = multiplicationGrpcServiceClient;
        _divisionGrpcSvcClient = divisionGrpcServiceClient;
        _subtractionGrpcSvcClient = subtractionGrpcServiceClient;
        _svcDiscoveryService = serviceDiscoveryService;
        _developmentServiceHost = developmentServiceHost;
        _refitService = refitService;
   //     _scopeProvider = scopeProvider;
        _logger = logger;
        //_publisher = publisher;
    }

    [HttpGet("GetHttpServiceConfiguration")]
    public async Task<OperationResult<HttpServiceConfiguration>> Get() =>
        await SendAsync(new GetHttpServiceConfigurationRequest { ServiceName = "test" });

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task<HttpServiceConfiguration> GetName(string name)
    {
        // var result = await _subtractionGrpcSvcClient.SubtractAsync(2, 1);

        // var first = _multiplicationGrpcSvcClient.MultiplyAsync(5, 4);
        // var second = _divisionGrpcSvcClient.DivideAsync(10, 5);

        // await Task.WhenAll(first, second);

        // await _publisher.Publish(new MyMessage { Number = 5 });

        var result = await _refitService.GetFilmDescriptionAsync(Guid.NewGuid());

        //await _developmentServiceHost.SendMessageOneWay2();

        //var result = await _multiplicationGrpcSvcClient.GetMultiplicationSequnceSumAsync(GetItems(), CancellationToken.None);

        //await foreach (var number in _multiplicationGrpcSvcClient.GetSequencePowerOfTwoAsync(new[] { 2, 4, 6, 8 }, CancellationToken.None))
        //{
        //    Console.WriteLine(number);
        //}

        return new HttpServiceConfiguration();
    }

    private async IAsyncEnumerable<(int, int)> GetItems()
    {
        var items = new[] { (1, 1), (2, 2), (3, 3), (4, 4) };

        foreach (var item in items)
        {
            yield return item;
        }
    }

    [HttpPut("UploadFile2")]
    public async Task<string> UploadFileAsync2(int file)
    {
        return "fff";
    }

    [HttpPut("UploadFile")]
    public async Task UploadFileAsync(IFormFile file)
    {
        return;
    }

    [HttpGet("OneWayTest")]
    public async Task GetOneWay(int number) => await SendAsync(new OneWayRequest { MyProperty = number });
}
