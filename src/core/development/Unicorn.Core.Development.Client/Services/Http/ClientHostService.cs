using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.Client.Features.GetHttpServiceConfiguration;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.Client.Services.Http;

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
    private readonly IServiceHostService _serviceHostService;
    //    private readonly IAuthenticationScope _scopeProvider;

    public ClientHostService(
        IServiceDiscoveryService serviceDiscoveryService,
        IMultiplicationGrpcServiceClient multiplicationGrpcServiceClient,
        IDivisionGrpcServiceClient divisionGrpcServiceClient,
        ISubtractionGrpcServiceClient subtractionGrpcServiceClient,
        IServiceHostService serviceHostService,
        //   IAuthenticationScope scopeProvider,
        ILogger<ClientHostService> logger
        //IUnicornEventPublisher publisher
        )
    {
        _multiplicationGrpcSvcClient = multiplicationGrpcServiceClient;
        _divisionGrpcSvcClient = divisionGrpcServiceClient;
        _subtractionGrpcSvcClient = subtractionGrpcServiceClient;
        _svcDiscoveryService = serviceDiscoveryService;
        _serviceHostService = serviceHostService;
        //     _scopeProvider = scopeProvider;
        _logger = logger;
        //_publisher = publisher;
    }

    [HttpGet("GetHttpServiceConfiguration")]
    public async Task<OperationResult<HttpServiceConfiguration>> Get() =>
        await SendAsync(new GetHttpServiceConfigurationRequest { ServiceName = "test" });

    [HttpGet("call-all-service-host-endpoints")]
    public async Task GetName(string name)
    {
        var result = await _serviceHostService.GetFilmDescriptionAsync(Guid.NewGuid());

        var result2 = await _serviceHostService.DeleteFilmDescriptionAsync(Guid.NewGuid());

        var result3 = await _serviceHostService.UploadFilmAsync(new FormFile(new MemoryStream(), 0, 0, "file", "file.txt")); ;

        var result4 = await _serviceHostService.UpdateFilmDescription(
            new ServiceHost.SDK.DTOs.FilmDescription
            {
                FilmId = Guid.NewGuid(),
                Description = "Description",
                DescriptionId = Guid.NewGuid(),
                Title = "Title",
                ReleaseDate = DateTime.Now,
                LastUpdatedOn = DateTime.Now,
            });
    }
}
