//using Microsoft.Extensions.Logging;
//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Playground.ServiceDiscovery.SDK;

//public interface IServiceDiscoveryClient
//{
//    Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName);
//    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName);
//}

//public class ServiceDiscoveryClient : IServiceDiscoveryClient
//{
//    private readonly string _baseUrl;
//    private readonly ILogger<ServiceDiscoveryClient> _logger;

//    public ServiceDiscoveryClient(string baseUrl, ILogger<ServiceDiscoveryClient> logger)
//    {
//        _baseUrl = baseUrl;
//        _logger = logger;
//    }

//    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName)
//    {
//        _logger.LogInformation($"Retrieving GRPC service configuration for: {serviceName}");
        
//        var client = new RestClient(_baseUrl);
//        var req = new RestRequest($"ServiceDiscovery?serviceName={serviceName}", Method.GET);

//        return await client.GetAsync<GrpcServiceConfiguration>(req);
//    }

//    public Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
//    {
//        throw new NotImplementedException();
//    }
//}
