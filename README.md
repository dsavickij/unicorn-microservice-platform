![unicorn](http://image-cdn.neatoshop.com/styleimg/66894/none/sand/default/371163-19;1512063557i.jpg)

# unicorn-project-microservices
Implementation of microservice architecture with inter-service communication abstracted from consumer as much as possible. Communication can be done using HTTP or high performance gRPC protocol.

gRPC support is not tested beyond simple implementation of it, so it is like 'beta' version.

The project is based on .NET 6.0

## Solution folder structure

* **core** - includes projects for inter-service communication over HTTP and gRPC as well as all the services required to achieve that
	* **infrastructure** - projects for inter-services communication
	* **services** - services required for inter-service communication (only ServiceDiscovery for now)
	* **development** - projects to facilitate development and testing of infrastructure projects and services. Projects in this folder reference infrastructure projects directly thus the need to create nugets for testing purpose is eliminated
* **services** - Unicorn microservices which consume infrastructure nuget packages and use core services for inter-service communication

## How does it work?

Every Unicorn microservice should provide SDK in the form of nuget package in order to let other microservices to call it. For microservice to call other microservice\'s HTTP or gRPC service only SDK and service configuration in ServiceDiscovery is needed. Of course, the caller is also required to use infrastructure packages.

### Creation of HTTP service

* Nuget package `Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http` needs to be added to SDK project
* HTTP service configuration needs to be registered in ServiceDiscovery service. Right now everything is hard coded in controller
* Assembly attribute `UnicornAssemblyServiceNameAttribute` with service name from ServiceDiscovery must be added to any class in SDK project
* HTTP service interface needs to be created. Methods defined in it must be implemented in Web API controller to receive request from microservices 
	* HTTP service interface needs to be decorated with `UnicornHttpServiceMarker` attribute
	* HTTP service interface methods need to be decorated with derivative of attribute `UnicornHttpAttribute` depending on what HTTP method needs to be used to call it. Every attribute need to provide URL path template. Respective ASP.NET HTTP method atrributes and path templates also need to be added to Web API controller.
   
HTTP service interface should look similar to this:

```c#
[UnicornHttpServiceMarker]
public interface IServiceDiscoveryService
{
    [UnicornHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceName);

    [UnicornHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> UpdateHttpServiceConfigurationAsync(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    Task DeleteHttpServiceConfigurationAsync(string serviceName);
}

```

Method signatures in Web API controller in HTTP service should look similar to this:

```c#
[ApiController]
public class ServiceDiscoveryController : ControllerBase, IServiceDiscoveryService
{
    private readonly ILogger<ServiceDiscoveryController> _logger;

    public ServiceDiscoveryController(ILogger<ServiceDiscoveryController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
    {
	// do your magic
    }

    [HttpPost("CreateHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration)
    {
	// do your magic
    }
}


```
### Creation of gRPC service client

* Nuget package `Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc` needs to be added to SDK
* SDK must have gRPC Proto file added to it. It is better to add it as a link to a file which is located in gRPC service itself 
* gRPC service configuration needs to be registered in ServiceDiscovery service. Right now everything is hard coded
* gRPC service client interface needs to be created
* gRPC service client interface must be decorated with `UnicornGrpcClientMarker` attribute
* gRPC service client implementation needs to be created
* gRPC service client implementation must inherit from gRPC service client interface and `BaseGrpcClient` abstract class. `BaseGrcpClient` will require to set `GrpcServiceName` property which must be identical to the registered gRPC service configuration in ServiceDiscovery

gRPC service client interface should look similar to this:

```c#
[UnicornGrpcClientMarker]
public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}

```
While gRPC service client implementation should look similar to this:

```c#
public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient? _client;

    protected override string GrpcServiceName => "GreeterProtoService";

    public GreeterProtoClient(IGrpcClientFactory factory)
        : base(factory)
    {
    }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.Call(GrpcServiceName, c => GetClient(c)!.SayHelloAsync(request));

    private Greeter.GreeterClient? GetClient(GrpcChannel channel) => _client ??= new Greeter.GreeterClient(channel);
}
```
### How service call is done?

Unicorn microservice host to be able to call other microservice from Unicorn universe needs to consume `Unicorn.Core.Infrastructure.SDK.HostConfiguration` nuget package and call `ApplyUnicornConfiguration` extension method on the host builder in `Program.cs`:

```c# 
builder.Host.ApplyUnicornConfiguration();
```

`ApplyUnicornConfiguration` extension method will scan assemblies for HTTP services and gRPC service clients and add them to dependency injection container. 

For HTTP service, proxy is created and registered to be resolved when HTTP service interface needs to be injected through class constructor. Proxy will intercept any invocation of HTTP service interface, transform it into HTTP request, send it, retrieve result and then propage it to the calling code.

For gRCP service client, gRPC service client interface and client implementation is registered.

On the first call to HTTP or gRPC service, service configuration from ServiceDiscovery service is retrieved and held in cache indefinitely for further reuse.

Afterwards, any HTTP service or gRCP service can be injected into any class using its interface:

```c#
    public WeatherForecastController(IServiceDiscoveryService serviceDiscoveryService)
    {
        _svcDiscoveryService = serviceDiscoveryService;
    }
```

And called just like any other object in the project:

```c#
    [HttpGet(Name = "Get")]
    public async Task<HttpServiceConfiguration> Get()
    {
        return await _svcDiscoveryService.GetHttpServiceConfiguration("myService");
    }
```

## Starting services
Microservices in _services_ folder require infrastructure nuget packages. These nugets are not published, so they need to be created locally and put in local nuget store on your machine.

Local nuget store can be added in Visual Studio settings just by selecting folder where nugets will be placed.

To create all the required nugets, change pathes and run these commands in Visual Studio\'s Developer Powershell:

```
dotnet pack 'C:\Users\dsavi\source\repos\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http\Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.csproj' --output 'C:\Users\dsavi\OneDrive\Dokumentai\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Users\dsavi\source\repos\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc\Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.csproj' --output 'C:\Users\dsavi\OneDrive\Dokumentai\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Users\dsavi\source\repos\unicorn-project-microservices\core\services\service-discovery\Unicorn.Core.Services.ServiceDiscovery.SDK\Unicorn.Core.Services.ServiceDiscovery.SDK.csproj' --output 'C:\Users\dsavi\OneDrive\Dokumentai\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Users\dsavi\source\repos\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.SDK.HostConfiguration\Unicorn.Core.Infrastructure.SDK.HostConfiguration.csproj' --output 'C:\Users\dsavi\OneDrive\Dokumentai\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Users\dsavi\source\repos\unicorn-project-microservices\services\Unicorn.GrpcService.SDK\Unicorn.GrpcService.SDK.csproj' --output 'C:\Users\dsavi\OneDrive\Dokumentai\Local NuGet Store' -p:PackageVersion=1.0.0
```
## Notes for further development

Possible plans for further learning/development:

* Implement several microservices with vertical slice architecture in mind. Use MediatR, FluentValidation, FeatureFolders (?), try using EF Core 6 without repository pattern, but direct injection of DBContext into classes
* Add unit tests: use FluentAssertions, Autofixture, xUnit
* Add authentication for inter-service communication. Research: Azure AD, Microsoft Identity
* Add Authorization? Investigate
* Ocelot or YARP for APIM/reverse-proxy/API gateway
* Something regarding messaging:
	* MassTransit on top of RabbitMQ message broker to try Saga pattern
	* Kafka for event sourcing to try what it can be used for
		* if decision to move current Unicorn architecture to event-driven will be made, create separate project 'pheonix-project-microservices' 	
* Elasticsearch just to see it in action
* Redis for distributed caching 
* Blazor for some UI and to have something to call API gateway
* Docker support in the form of single command to launch all microservices in containers
* Add system monitoring? Prometheus, Grafana, checkout HealthChecks, etc.

## Links

* Link to markdown (\*.md) file editor: https://markdown-editor.github.io/
* Link to markdown syntax: https://medium.com/analytics-vidhya/how-to-create-a-readme-md-file-8fb2e8ce24e3
