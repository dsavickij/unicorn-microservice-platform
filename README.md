![unicorn](http://image-cdn.neatoshop.com/styleimg/66894/none/sand/default/371163-19;1512063557i.jpg)

# unicorn-microservice-platform
Unicorn is a platform for development of microservices. The platform consists of shared components for infrastructure and platform-wide common parts, ensuring that no microservice will ever need to 'invent' them again. This separation of concerns allows the software engineer to focus on development of the features in microservice, thus speeding up the overall process.

Unicorn platform shared components include authentication, input data validation, configuration, inter-service communication and enforcement of vertical slice architectural style.

The project is based on .NET 6.0 and is in constant development.

## Features

* **Support for one-way (asynchronous) communication**
	* Inter-service communication using message queues and events (topics) is supported using RabbitMQ or Azure ServiceBus message broker 	
* **Support for two-way (synchronous) communication**
	* Inter-service communication is supported over HTTP or high performance gRPC protocols  	
* **Microservice SDK nuget is all what is needed**
	* Microservice's SDK contains everything what is required for consuming service to successfully call it
*  **Inlucded Unicorn project template**
	* New Unicorn microservice can be created from included project template installed in Visual Studio/Rider. Few clicks and all required projects are addedto solution	
* **High level of abstraction**
	* Owner of microservice does not need to write any code to ensure microservice operation as that come "out of the box" from infrastructure packages	
* **Enforcement of vertical slice architecture pattern**
	* Base class for HTTP service Web API controller includes MediatR out of the box and pushes software engineer to develop microservice in the spirit of vertical slice architecture
* **Included API gateway**
	* All requests to Unicorn microservices can go through included API gateway acting as a reversed proxy with such capabilities like rate limiting
*  **Included service discovery**
	* Microservice HTTP ang gRPC configurations are centralized in one place	
* **Common  types for respones**
	* Infrastructure packages includes types `OperationResult` and `OperationResult<T>` to use as common responses across all microservices

## (Very) Brief overview of technical implementation

Unicorn platform is done with idea to speed up development of new microservices. The speed is achieved by concentrating the most important components and services in __Unicorn.Core.*__ projects and providing their funcionality as nuget packages. Nuget packages is also the primary way to do to service to service communication as these nugets include HTTP service contracts, gRPC service clients, events and service name. Thus, for microservice to call another microservice, the former is required to consume the SDK nuget of the later.

During the startup of Unicorn microservice, the appliciation scans librararies and registers HTTP services, gRPC service clients, event handlers, data validation classes and starts to listen to message broker queues for the one-way endpoints the microservice exposes.

During the request, the caller uses service host name in target service SDK to get configuration from Service Discovery service. If HTTP service is being called, HTTP configuration is retrieved and cached. If the gRPC service is called - the same is done for gRPC configuration. 

For proper functioning of microservice, certain configuration is required to be provided in microservice configuration. That include Service Discovery service URL for service configuration retrieval, subscription identifier for subscription to message broker topics and message broker connection string.

## Tech-stack

This section contains the overview of nuget packages, databases and design desicions used in this repository.

* **Microservice inter-service communication is implemented using orchestration pattern**
* **Microservices use vertical slice architectural approach**
* **Docker is used for containerization**
* **Main nuget packages used in solution:**
	*  RestSharp, Castle.Core, GuardClauses, MassTransit, FluentValidation, MediatR, Swashbuckle (Swagger), OneOf, OpendIddict (for authentication), Entity Framework Core 6, Autofixture, xUnit, FluentAssertions.
* **Databases:**
	* PostgreSQL (Unicorn.eShop.Cart microservice)
	* Microsoft SQL 2019 (Unicorn.Core.Services.ServiceDiscovery microservice)   

## Table of contents
- [Repository structure](#repository-structure)
- [Getting Unicorn.eShop microservices started](#getting-unicorneshop-microservices-started)
	- [Creating local nuget packages](#creating-local-nuget-packages)
	- [Launching microservices in Docker containers](#launching-microservices-in-docker-containers)
	- [URLs to Unicorn.eShop microservices](#urls-to-unicorneshop-microservices)
- [Creation of new Unicorn microservice](#creation-of-new-unicorn-microservice)
	- [Web API host configuration](#web-api-host-configuration)
	- [Adding HTTP service](#adding-http-service)
		- [Adding two-way endpoints](#adding-two-way-endpoints)
		- [Adding of one-way endpoints](#adding-one-way-endpoints) 
	- [Adding gRPC service](#adding-grpc-service)
	- [Adding events](#adding-events)
- [Inter-service communication](#inter-service-communication)
	- [Calling HTTP service endpoints](#calling-http-service-endpoints)
	- [Calling gRPC service endpoints](#calling-grpc-service-endpoints)
	- [Subscribing to and handling of events](#subscribing-to-and-handling-of-events) 	
- [Notes for further development](#notes-for-further-development)
- [Links](#links)


## Repository structure

* **core** - includes projects of Unicorn platform
	* **infrastructure** - projects for inter-services communication, data validation, authentication, service registration, etc.
	* **services** - independent services required to ensure the work of microservices (service-discovery, authentcation, etc.)
	* **development** - projects to facilitate development and testing of __core__ projects and services. These projects reference infrastructure projects directly to speed up development and testing by removing the need to create new nugets for even small changes
* **eShop** - example e-commerce microservices built on top of Unicorn platform. Right now these projects include only back-end services in early stage of development.

## Getting Unicorn.eShop microservices started

Unicorn.eShop microservices is a collection of e-commerce services serving as an example of what is possible to make using Unicorn platform. At the moment there are several microservices for Unicorn.eShop e-commerce solution. These microservices are containerized and can be started-up without installation of any database or message broker. Yet, several things still needs to be done to launch them.

### Creating local nuget packages

Unicorn.eShop miroservices use Unicorn platform nuget packages to configure the host and have required components to ensure their operations. The nugets are not pushed to public repository, so they need to be created manually and stored in local nuget store.

To create local nuget packages, after changing pathes to projects and `--output` to your own, in Visual Studio's Developer Powershell (or just standalone Powershell) run the following commands:

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
### Launching microservices in Docker containers

Unicorn.eShop services are containerized and require Docker Desktop to start them. It is possible to not to use Docker, but that requires manual alterations in service configuration files and installation of message broker and databases.

If Docker Desktop is not installed on your machine, please download and intall it. After that, open Unicorn-project-microservices solution in you IDE and set __docker-compose__ as default startup project. Now, start the solution: Unicorn.eShop services will start on Docker.

### URLs to Unicorn.eShop microservices

Unicorn.eShop microservice HTTP APIs can be accessed by the following URLs:

* **Unicorn.eShop.Cart** - https://localhost:8001/swagger/index.html
* **Unicorn.eShop.Discount** - https://localhost:8005/swagger/index.html
* **Unicorn.eShop.Catalog** - https://localhost:8007/swagger/index.html

Unicorn.Core.Services:

* **Unicorn.Core.Service.ServiceDiscovery** - https://localhost:8003/swagger/index.html


## Creation of new Unicorn microservice

Every Unicorn microservice should provide SDK in the form of nuget package in order to let other microservices to call it. For microservice to call other microservice\'s HTTP or gRPC service only SDK and service configuration in ServiceDiscovery is needed. Of course, the caller is also required to use Unicorn platform nuget packages.

Typical Unicorn microservice consists of at least 1 Web API project and 1 class libarary for SDK.

### Web API host configuration

1. Add `Unicorn.Core.Infrastructure.HostConfiguration.SDK` nuget package to created Web API project
2. Go to `Program.cs`, remove every line of code and paste the following:

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

### Adding HTTP service

1. Add `Unicorn.Core.Infrastructure.Communication.Http.SDK` nuget package to SDK project
2. If it was not added before, add assembly attribute `UnicornServiceHostNameAttribute` with service host name to SDK project. Service host name is a key by which HTTP configuration will be retrieved from Service Discovery service by other microservices
3. Register HTTP service configuration with service host name in ServiceDiscovery service
4. Create HTTP service interface. Methods defined in it will be implemented in Web API controller to receive request from microservices 
5. Decorate HTTP service interface with `UnicornHttpServiceMarkerAttribute` attribute
   
HTTP service interface with all above mentioned attributes should look similar to this:

```c#
[assembly: UnicornServiceHostName("Unicorn.eShop.Cart")]

namespace Unicorn.eShop.Cart.SDK;

[UnicornHttpServiceMarker]
public interface ICartService
{
}

```

HTTP service interface provided in microservice SDK must be implemented by the microservice to handle requests. To achieve that, a Web API controller needs to be created. This controller must inherit from `UnicornBaseController<>` class and provide HTTP service interface as a generic parameter to it. 

After everything is done, a Web API controller should look similar to this:

```c#
public class CartServiceController : UnicornBaseController<ICartService>, ICartService
{
    private readonly ILogger<CartServiceController> _logger;

    public CartServiceController(ILogger<CartServiceController> logger)
    {
        _logger = logger;
    }
}

```
#### Adding two-way endpoints

Two-way HTTP service endpoints are defined in HTTP service interface. There is support for GET, POST, PUT and DELETE methods.

To add two-way endpoint, the following must be done:

1. Open HTTP service interface in microservice's SDK project
2. Add method signature
3. Decorate method signature with Unicorn HTTP method attribute: ```UnicornHttpGetAttribute``` for GET method, ```UnicornHttpPostAttribute``` for POST method and so on. These attributes require path to be provided for their constructors
4. If method signature has parameters, add data binding attributes like ```UnicornFromRouteAttribute```, ```UnicornFromBodyAttribute``` besides them
5. After everything is done, HTTP service interface with added two-way endpoints should look similar to this:

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

6. Next, implement endpoints defined in HTTP service interface in microservice controller 
7. Change Unicorn attributes to ASP.NET equivalents. For exmmple, change ```UnicornHttpPostAttribute``` to ```HttpPostAttribute```
8. After everything is done, controller should look something like this:

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
	// do your magic
    }

    [HttpGet("api/carts/{cartId}/discounts/{discountCode}")]
    public async Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync(
        [FromRoute] Guid cartId, [FromRoute] string discountCode)
    {
	// do your magic
    }

    [HttpGet("api/carts/my")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
	// do your magic
    }

    [HttpDelete("api/carts/{cartId}/items/{itemId}/remove")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid cartId, [FromRoute] Guid itemId)
    {
	// do your magic
    }
}
```

#### Adding one-way endpoints

One-way HTTP service endpoints are defined in HTTP service interface.

To add one-way endpoint, the following must be done:

1. Open HTTP service interface in microservice's SDK project
2. Add method signature
3. Decorate method signature with Unicorn one-way method attribute ```UnicornOneWayAttribute```
5. After everything is done, HTTP service interface with added one-way endpoint should look similar to this:

```c#
[assembly: UnicornServiceHostName("Unicorn.eShop.Cart")]

namespace Unicorn.eShop.Cart.SDK;

[UnicornHttpServiceMarker]
public interface ICartService
{
    [UnicornOneWay]
    Task SendMessageOneWay();
}
```
6. Next, in Web API project add one-way method to controller which implements HTTP service interface
7. Decorate added endpoint with APS.NET ```NonActionAttribute``` attribute
8. After everything is done, controller should look similar to this:

```c#
public class CartServiceController : UnicornBaseController<ICartService>, ICartService
{
    private readonly ILogger<CartServiceController> _logger;

    public CartServiceController(ILogger<CartServiceController> logger)
    {
        _logger = logger;
    }

    [NonAction]
    public async Task SendMessageOneWay()
    {
	// do your magic
    }
}
```
9. Issue new version of microservice SDK nuget package and consumers will be able use newly defined one-way endpoint.

### Adding gRPC service

1. Add `Unicorn.Core.Infrastructure.Communication.Grpc.SDD` nuget package needs to SDK project
2. Add gRPC service Proto file to SDK project and provide the service definiton: methods, message types, etc.
3. Add the following lines in SDK projet's `*.csproj` file:

```xml
<ItemGroup>
	<PackageReference Include="Google.Protobuf" Version="3.19.2" />
	<PackageReference Include="Grpc.Tools" Version="2.43.0">
		<PrivateAssets>all</PrivateAssets>
		<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
	</PackageReference>
</ItemGroup>
```

4. As SDK project will be consumed by the microservice's Web API project as well by other microservices through nuget, compilation services need to be set to generate gRPC service and client code. To achieve that, slightly change and add the following lines to SDK `*.csproj` file:

```xml
<ItemGroup>
  <Protobuf Include="<RELATIVE PATH TO PROTO FILE>" GrpcServices="Server, Client" />
</ItemGroup>
```

5. Register gRPC service configuration ServiceDiscovery service.
6. In SDK project, create  gRPC service client interface
	* gRPC service client interface must be decorated with `UnicornGrpcClientMarkerAttribute` attribute

gRPC service client interface should look similar to this:

```c#
[UnicornGrpcClientMarker]
public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}
```

7. In SDK project, create gRPC service client
	* gRPC service client implementation must inherit from gRPC service client interface and `BaseGrpcClient` abstract class

gRPC service client implementation should look similar to this:

```c#
public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient? _client;

    public GreeterProtoClient(IGrpcClientFactory factory)
        : base(factory)
    {
    }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.Call(GrpcServiceName, c => new Greeter.GreeterClient(c)!.SayHelloAsync(request));
}
```
9. In microservice's Web API project, create gRPC service implementation by inheriting from base class generated after SDK project compilation. This service will be handling requests sendt from gRPC client

This service should like similar to this:

```c#
public class GreeterGrpcService : GreeterProto.GreeterProtoBase
{
    public override async Task<SayHelloReply> SayHelloAsync(SayHelloRequest request, ServerCallContext context)
    {
	// do your magic
    }
}
```
10. Add gRPC service in microservice's Web API project `Program.cs`:

```c#
app.MapGrpcService<GreeterGrpcService>();
```

After that, if everything was done correctly, gRPC service is ready to be used. All is needed to ocnsume it is to issue microservice SDK nuget.

#### Adding events

Unicorn platform supports events using RabbitMQ and Azure ServiceBus message brokers. Event is a simple data class/record in microservice's SDK project, which consumer receives if it has subsribed to it by creating consumer. 

The event class/record is used to publish event and to subsribe to it for its receival. 

Event can look something like this:

```c#
public record MyEvent
{
    public int Number { get; set; }
}

```

Event publising is done by resolving ```IUnicornEventPublisher``` through dependency injection and providing event instance to its only method. Code for event publishing looks like this:

```c#
public class ClientHostController : UnicornBaseController<IClientHostService>, IClientHostService
{
    private readonly IUnicornEventPublisher _publisher;

    public ClientHostController(IUnicornEventPublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task GetName(string name)
    {
        await _publisher.Publish(new MyEvent { Number = 5 }););
    }
}
```
After event is published, any consumer which has defined its consumer, will receive it. **Information on how to receive event can be found here**.

## Inter-service communication

In Unicorn platform inter-service communication when one microservice tries to communicate with the other requires destination service SDK in the form of nuget package. This nuget contains HTTP and gRPC services, as well as events to which consumer can subscribe. 

During the startup of microservice, the host scans referenced SDKs and registers HTTP and gRPC services in dependency injection container. That means that to call an HTTP or gRPC service, its interface needs to be injected into the class. After injection, the destination endpoint is called just like any other method as execution will be taken care of by Unicorn infrastructure nuget packages.

### Calling HTTP service endpoints

1. Add destination microservice SDK nuget package to Web API project
2. Inject HTTP service interface into the class which will call the destination endpoint
3. Call the destination HTTP service endpoint

After everything was done, the code to call HTTP service endpoint may look similar to this:

```c#
public class ClientHostController : UnicornBaseController<IClientHostService>, IClientHostService
{
    private readonly IHttpService _httpService;

    public ClientHostController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task GetName(string name)
    {
        var response = await _httpService.SendMessage(new Message() { Id = 1 });
    }
}
```

### Calling gRPC service endpoints

1. Add destination microservice SDK nuget package to Web API project
2. Inject gRPC service client interface into the class which will call the destination endpoint
3. Call the destination gRPC service client endpoint

After everything was done, the code to call gRPC service endpoint may look similar to this:

```c#
public class ClientHostController : UnicornBaseController<IClientHostService>, IClientHostService
{
    private readonly IMultiplicationGrpcServiceClient _multiplicationGrpcSvcClient;

    public ClientHostController(IMultiplicationGrpcServiceClient multiplicationGrpcServiceClient
    {
        _multiplicationGrpcSvcClient = multiplicationGrpcServiceClient;
    }

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task GetName(string name)
    { 
        var response = await _multiplicationGrpcSvcClient.MultiplyAsync(5, 4);
    }
}

```

### Subscribing to and handling of events

For successfull subscription to events, proper configuration in ```appSettings.json``` is required. The section ```<YOUR SERVICE HOST SETTINGS NAME>:OneWayCommunicationSettings``` needs to have correct values for message broker connection string and subscription identifier.

To subscribe to remote microservice event, the following needs to be done:

1. Add microservice SDK, which includes the event, nuget package to Web API project
2. Create event handler by creating a class which implements ```IUnicornEventHandler<>``` interface. The interface accepts generic type parameter which needs to be event from SDK - this will ensure that subscription for this type of events will be created

After everything was done, the event handler code should look similar to this:

```c#
public class MyMessageHandler : IUnicornEventHandler<MyMessage>
{
    public Task Consume(ConsumeContext<MyMessage> context)
    {
	// do your magic
    }
}
```

## Notes for further development

Possible plans for further learning/development:

* Implement several microservices with vertical slice architecture in mind. Use MediatR, FluentValidation, FeatureFolders (?), try using EF Core 6 without repository pattern, but direct injection of DBContext into classes
* ~~Add unit tests: use FluentAssertions, Autofixture, xUnit~~ Several unit tests added with utilization of mentioned nugets, code coverage still needs to be improved
* ~~Add authentication for inter-service communication. Research: Azure AD, Microsoft Identity~~ Done. Used OpenIddict for implicit and client credentials authentication flows
* Add Authorization? Investigate
* ~~Ocelot or YARP for APIM/reverse-proxy/API gateway~~ Added Api gateway using Ocelot. YARP version can be created and tested in the future, too
* Something regarding messaging:
	* ~~MassTransit on top of RabbitMQ message broker to try Saga pattern~~ MassTransit on top of RabbitMQ and Azure ServiceBus was added with no issues. Saga pattern testing if left for the future
	* Kafka for event sourcing to try what it can be used for
		* if decision to move current Unicorn architecture to event-driven will be made, create separate project 'pheonix-project-microservices' 	
* Elasticsearch just to see it in action
* Redis for distributed caching
* Add Polly for request retry logic and exponential back-off
* Blazor for some UI and to have something to call API gateway
* ~~Docker support in the form of single command to launch all microservices in containers~~ Docker-compose project is added and used with great success
* Add system monitoring? Prometheus, Grafana, checkout HealthChecks, etc.
* Add CI/CD with SonarCloud static code analysis and code coverage calculation
* Add .NET project templates for creating microservice host and SDK projects

## Links

* Link to markdown (\*.md) file editor: https://markdown-editor.github.io/
* Link to markdown syntax: https://medium.com/analytics-vidhya/how-to-create-a-readme-md-file-8fb2e8ce24e3
