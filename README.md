![unicorn](http://image-cdn.neatoshop.com/styleimg/66894/none/sand/default/371163-19;1512063557i.jpg)

# unicorn-project-microservices
Implementation of microservice architecture with common components, required for every microservice for its operations, concentrated in separate projects and consumed as nuget packages. The project puts emphasis on making the development of new microservices as fast as possible and it achieves that by pushing many components (authentication, data validation, inter-service communication, enforcement of vertical slice architectural style on microservice, etc.) out of concerns of the team working on new microservice. 

The project is based on .NET 6.0 and is in constant development.

## Features

* **Support for one-way (asynchronous) communication**
	* Inter-service communication using message queues and events (topics) is supported using RabbitMQ or Azure ServiceBus message broker 	
* **Support for two-way (synchronous) communication**
	* Inter-service communication is supported over HTTP or high performance gRPC protocols.  	
* **Microservice SDK nuget is all what is needed**
	* Microservice's SDK contains everything what is required for consuming service to successfully call it
* **High level of abstraction**
	* Owner of microservice does not need to write any code to ensure microservice operation as that come "out of the box" from infrastructure packages	
* **Enforcement of vertical slice architecture pattern**
	* Base class for HTTP service Web API controller includes MediatR out of the box and pushes software engineer to develope microservice in the spirit of vertical slice architecture
* **Common  types for respones**
	* Infrastructure packages includes types `OperationResult` and `OperationResult<T>` to use as common responses across all microservices

## (Very) Brief overview of technical implementation

Unicorn project is done with idea to speed up development of new microservices. The speed is achieved by concentrating the most important components and services in __Unicorn.Core.*__ projects and providing their funcionality as nuget packages. Nuget packages is also the primary way to do to service to service communication as these nugets are required to include HTTP service contracts, gRPC service clients, events and service name. Thus, for microservice to call another microservice, the former is required to consume the SDK nuget of later.

During the startup of Unicorn microservice, the appliciation scans librararies and registers HTTP services, gRPC service clients, event handlers, data validation classes and starts to listen to message broker queues for the one-way endpoints the microservice exposes.

During the request, the caller uses service host name in target service SDK to get configuration from Service Discovery service. If HTTP service is being called, HTTP configuration is retrieved and cached. If the gRPC service is called - the same is done for gRPC configuration. 

For proper functioning of microservice, certain configuration is required to be provided in microservice configuration. That include Service Discovery service URL for service configuration retrieval, subscription identifier for subscription to message broker topics and message broker connection string.

## Table of contents
- [Repository structure](#repository-structure)
- [Getting Unicorn.eShop microservices started](#getting-unicorneshop-microservices-started)
	- [Creation of local nuget packages](#creation-of-local-nuget-packages)
	- [Lauching all services in Docker containers](#lauching-all-services-in-docker-containers)
	- [URLs to Unicorn.eShop microservices](#urls-to-unicorneshop-microservices)
- [Creation of a new Unicorn microservice](#creation-of-a-new-unicorn-microservice)
	- [Web API host configuration](#web-api-host-configuration)
	- [Addition of HTTP service](#addition-of-http-service)
	- [Addition of gRPC service client](#addition-of-grpc-service-client)
	- [How service call is done?](#how-service-call-is-done)
- [Notes for further development](#notes-for-further-development)
- [Links](#links)


## Repository structure

* **core** - includes projects serving as a foundation for building microservices
	* **infrastructure** - projects for inter-services communication, data validation, authentication, service registration, etc.
	* **services** - independent services required to ensure the work of microservices (service-discovery, authentcation, etc.)
	* **development** - projects to facilitate development and testing of __core__ projects and services. These projects reference infrastructure projects directly to speed up development and testing by removing the need to create new nugets for even small changes
* **eShop** - e-commerce microservices built on top of __core/infrastructure__ projects and using __core/services__ in their operations. Right now these projects include only back-end services in early stage of development.

## Getting Unicorn.eShop microservices started

Unicorn.eShop microservices is a collection of e-commerce services serving as an example of what is possible to make using Unicorn infrastrcuture packages. At the moment there are several microservices for Unicorn.eShop e-commerce solution. These microservices are containerized and can be started-up without installation of any database or message broker. Yet, several things still needs to be done to launch them.

### Creation of local nuget packages

Unicorn.eShop miroservices use infrastrucuture nuget packages to configure the host and have required components to ensure their operations. The nugets are not pushed to public repository, so they need to be created manually and stored in local nuget store.

To create local infrastructure packages, after changing pathes to projects and `--output` to your own, in Visual Studio's Developer Powershell (or just standalone Powershell) run the following commands:

```c#
dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.Communication.Common\Unicorn.Core.Infrastructure.Communication.Common.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.Communication.Grpc.SDK\Unicorn.Core.Infrastructure.Communication.Grpc.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.Communication.Http.SDK\Unicorn.Core.Infrastructure.Communication.Http.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.Communication.MessageBroker\Unicorn.Core.Infrastructure.Communication.MessageBroker.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.Security.IAM\Unicorn.Core.Infrastructure.Security.IAM.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\services\service-discovery\Unicorn.Core.Services.ServiceDiscovery.SDK\\Unicorn.Core.Services.ServiceDiscovery.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\core\infrastructure\Unicorn.Core.Infrastructure.HostConfiguration.SDK\Unicorn.Core.Infrastructure.HostConfiguration.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

```
After finishing, Powershell will create project nuget packages in `--output` folder. This folder needs to be added as a local nuget store in Visual Studio. [Here](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources) you can find how to do that.

Inter-service communication between Unicorn microservices is done utilising data provided in SDKs. Thus, Unicorn.eShop service nugets are also required to be created. Once again, change the pathes and run the following commands:

```c#
dotnet pack 'C:\Src\unicorn-project-microservices\eShop\discount\Unicorn.eShop.Discount.SDK\Unicorn.eShop.Discount.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

dotnet pack 'C:\Src\unicorn-project-microservices\eShop\catalog\Unicorn.eShop.Catalog.SDK\Unicorn.eShop.Catalog.SDK.csproj' --output 'C:\Users\dsavi\Documents\Local NuGet Store' -p:PackageVersion=1.0.0

```
### Lauching all services in Docker containers

Unicorn.eShop services are containerized and require Docker Desktop to start them. It is possible to not to use Docker, but that requires manual alterations in service configuration files and installation of message broker and databases.

If Docker Desktop is not installed on your machine, please download and intall it. After that, open Unicorn-project-microservices solution in you IDE and set __docker-compose__ as default startup project. Now, start the solution: Unicorn.eShop services will start on Docker.

### URLs to Unicorn.eShop microservices

Unicorn.eShop microservice HTTP APIs can be accessed by the following URLs:

* **Unicorn.eShop.Cart** - https://localhost:8001/swagger/index.html
* **Unicorn.eShop.Discount** - https://localhost:8005/swagger/index.html
* **Unicorn.eShop.Catalog** - https://localhost:8007/swagger/index.html

Unicorn.Core.Services:

* **Unicorn.Core.Service.ServiceDiscovery** - https://localhost:8003/swagger/index.html



## Creation of a new Unicorn microservice

Every Unicorn microservice should provide SDK in the form of nuget package in order to let other microservices to call it. For microservice to call other microservice\'s HTTP or gRPC service only SDK and service configuration in ServiceDiscovery is needed. Of course, the caller is also required to use infrastructure packages.

Typical Unicorn microservice consists of at least 1 Web API project and 1 class libarary for SDK.

### Web API host configuration

1. Add `Unicorn.Core.Infrastructure.SDK.HostConfiguration` nuget package to created Web API project
2. Go to `Program.cs` and remove every line of code here and paste the following:

```c#
var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Host.ApplyUnicornConfiguration<ServiceHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();

```

3. `ServiceHostSettings` in `builder.Host.ApplyUnicornConfiguration<ServiceHostSettings>();` represents the configuration record for microservice. This record must inherit from BaseHostSettings and look similar to this:

```c#
public record ServiceHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
```
5. `ServiceHostSettings` can contain additional configuration specific for the service. Above you can see it contains `DbConnectionString` property
6. Go to `appSettings.Development.json` and add the following configuration:

```json
  "ServiceHostSettings": {
    "DbConnectionString": "Server=production;Port=5432;Database=CartDb;User Id=admin;Password=admin1234;",
    "ServiceDiscoverySettings": {
      "Url": "http://localhost:5081"
    },
    "OneWayCommunicationSettings": {
      "ConnectionString": "<INSERT RABBITMQ CONNECTION STRING HERE>",
      "SubscriptionId": "6f5b8662-f95c-4951-ba4a-bf9ab94f7b7a"
    },
    "AuthenticationSettings": {
      "AuthorityUrl": "https://localhost:44319/",
      "ClientCredentials": {
        "ClientId": "console",
        "ClientSecret": "388D45FA-B36B-4988-BA59-B187D329C207",
        "Scopes": ""
      }
    }
  }
```
7. In JSON above you need to:
	*  add  `ServiceHostSettings:ServiceDiscoverySettings:Url` value for Service Discovery service
	*  add `ServiceHostSettings:OneWayCommunicationSettings:ConnectionString` value for message broker
	*  add `ServiceHostSettings:OneWayCommunicationSettings:SubscirptionId` value for topic subscription to receive events
	*  leave `ServiceHostSettings:AuthenticationSettings` as is because authentication is disabled due to certificate issues while running application in Docker
8. Copy of this JSON configuration also needs to be added to `appSettings.json` if the microservice will ever be launched using Release configuration

That's all what is required to configure new microservice to use __Unicorn.Core.*__ infrastructure packages.

### Addition of HTTP service

* Nuget package `Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http` needs to be added to SDK project
* Assembly attribute `UnicornServiceHostName` with service host name must be added to any class in SDK project. Srvice host name is a key by which HTTP configuration will be retrieved from Service Discovery service by other microservices
* HTTP service configuration with service host name needs to be registered in ServiceDiscovery service
* HTTP service interface needs to be created. Methods defined in it must be implemented in Web API controller to receive request from microservices 
	* HTTP service interface needs to be decorated with `UnicornHttpServiceMarker` attribute
	* HTTP service interface methods need to be decorated with derivative of attribute `UnicornHttpAttribute` depending on what HTTP method needs to be used to call it. Every attribute need to provide URL path template. Respective ASP.NET HTTP method atrributes and path templates also need to be added to Web API controller.
   
HTTP service interface with all above mentioned attributes should look similar to this:

```c#
[assembly: UnicornServiceHostName("Unicorn.eShop.Cart")]

namespace Unicorn.eShop.Cart.SDK;

[UnicornHttpServiceMarker]
public interface ICartService
{
    [UnicornHttpPost("api/carts/{cartId}/items/add")]
    Task<OperationResult> AddItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromBody] CartItemDTO cartItem);

    [UnicornHttpDelete("api/carts/{cartId}/items/{itemId}/remove")]
    Task<OperationResult> RemoveItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromRoute] Guid itemId);

    [UnicornHttpGet("api/carts/my")]
    Task<OperationResult<CartDTO>> GetMyCartAsync();

    [UnicornHttpGet("api/carts/{cartId}/discounts/{discountCode}")]
    Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync([UnicornFromRoute] Guid cartId, [UnicornFromRoute] string discountCode);
}

```

HTTP service interface provided in microservice SDK must be implemented by the microservice to handle requests. To achieve that, a Web API controller needs to be created. This controller must inherit from `UnicornBaseController<>` class and provide HTTP service interface as a generic parameter to it. 

After everthing is done, a Web API controller should look similar to this:

```c#
public class CartServiceController : UnicornBaseController<ICartService>, ICartService
{
    private readonly ILogger<CartServiceController> _logger;

    public CartServiceController(ILogger<CartServiceController> logger)
    {
        _logger = logger;
    }

    [HttpPost("api/carts/{cartId}/items/add")]
    public async Task<OperationResult> AddItemAsync([FromRoute] Guid cartId, [FromBody] CartItemDTO cartItem)
    {
        return await SendAsync(new AddItemRequest
        {
            CartId = cartId,
            Item = cartItem
        });
    }

    [HttpGet("api/carts/{cartId}/discounts/{discountCode}")]
    public async Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync(
        [FromRoute] Guid cartId, [FromRoute] string discountCode)
    {
        return await SendAsync(new ApplyDiscountRequest
        {
            CartId = cartId,
            DiscountCode = discountCode
        });
    }

    [HttpGet("api/carts/my")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
        return await SendAsync(new GetMyCartRequest());
    }

    [HttpDelete("api/carts/{cartId}/items/{itemId}/remove")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid cartId, [FromRoute] Guid itemId)
    {
        return await SendAsync(new RemoveItemRequest
        {
            CartId = cartId,
            CatalogItemId = itemId
        });
    }
}

```
### Addition of gRPC service client

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
* ~~Add authentication for inter-service communication. Research: Azure AD, Microsoft Identity~~ Done. Used OpenIddict for implicit and client credentials authentication flows
* Add Authorization? Investigate
* Ocelot or YARP for APIM/reverse-proxy/API gateway
* Something regarding messaging:
	* ~~MassTransit on top of RabbitMQ message broker to try Saga pattern~~ MassTransit on top of RabbitMQ and Azure ServiceBus was added with no issues. Saga pattern testing if left for the future
	* Kafka for event sourcing to try what it can be used for
		* if decision to move current Unicorn architecture to event-driven will be made, create separate project 'pheonix-project-microservices' 	
* Elasticsearch just to see it in action
* Redis for distributed caching 
* Blazor for some UI and to have something to call API gateway
* ~~Docker support in the form of single command to launch all microservices in containers~~ Docker-compose project is added and used with great success
* Add system monitoring? Prometheus, Grafana, checkout HealthChecks, etc.

## Links

* Link to markdown (\*.md) file editor: https://markdown-editor.github.io/
* Link to markdown syntax: https://medium.com/analytics-vidhya/how-to-create-a-readme-md-file-8fb2e8ce24e3
