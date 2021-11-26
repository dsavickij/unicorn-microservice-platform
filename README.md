![unicorn](http://image-cdn.neatoshop.com/styleimg/66894/none/sand/default/371163-19;1512063557i.jpg)

# unicorn-project-microservices
Implementation of microservice architecture with inter-service communication abstracted as much as possible. Communication can be done using HTTP or high performance gRPC protocol.

gRPC support is not tested beyond simple implementation of it, so it is like 'alpha' version.

The project is based on .NET 6.0

## how does it work?

Every Unicorn microservice should provide SDK in the form of nuget package in order to let other microservices to call it.

In SDK:
* For HTTP service:
	* Nuget package `Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http` needs to be added
	* HTTP service configuration needs to be registered in ServiceDiscovery service. Right now everything is hard coded
	* Assembly attribute `UnicornAssemblyServiceNameAttribute` with service name from ServiceDiscovery must be added
	* HTTP service interface needs to be decorated with `UnicornHttpServiceMarker` attribute
	* HTTP service interface methods need to be decorated with derivative of attribute `UnicornHttpAttribute` with URL path template
* For gRPC service client:
	* Nuget package `Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc` needs to be added
	* SDK must have gRPC PROTO file added to it. It is better to add it as a link to a file which is located in gRPC service itself 
	* gRPC service configuration needs to be registered in ServiceDiscovery service. Right now everything is hard coded
	* gRPC service client interface must be decorated with `UnicornGrpcClientMarker` attribute
	* gRPC service client implementation must inherit from gRPC service client interface and `BaseGrpcClient` abstract class. `BaseGrcpClient` will require to set `GrpcServiceName` property which must be identical to the registered gRPC service configuration in ServiceDiscovery

Unicorn micorservice host to be able call other microservice from Unicorn universe needs to consume `Unicorn.Core.Infrastructure.SDK.HostConfiguration` nuget pacakge and `ApplyUnicornConfiguration` extension method on the host builder in `Program.cs`:

`builder.Host.ApplyUnicornConfiguration();`

`ApplyUnicornConfiguration` extension method will scan assemblies for HTTP and gRPC service clients and add them to dependency injection container. 

For HTTP service, proxy is created and registered to be resolved when HTTP service interface needs to be injected through class constructor. Proxy will intercept any invocation of HTTP service interface, transform it in HTTP request, send it, retrieve result propage it to the calling code.

For gRCP service client, gRPC service client interface and client implementation is registered.

Afterwards, any HTTP service or gRCP service can be injected into any class using its interface:

```
    public WeatherForecastController(IServiceDiscoveryService serviceDiscoveryService)
    {
        _svcDiscoveryService = serviceDiscoveryService;
    }
```


## solution folder structure

* **core** - includes projects for inter-service communication over HTTP and gRPC as well as all the services required to achieve that
	* **infrastructure** - projects for inter-services communication
	* **services** - services required for inter-service communication (only ServiceDiscovery for now)
	* **pocs** - projects to facilitate development and testing of infrastructure projects and services. Projects in this folder reference infrastructure projects directly thus the need to create nugets for testing only is eliminated
* **services** - Unicorn microservices which consume infrastructure nuget packages and use core services for inter-service communication

## starting services
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
## notes for further development

Possible plans for further learning/development:

* Move microservice scope architecture towards vertical slice architecture with MediatR, FluentValidation, FeatureFolders (?), try using EF Core 6 without repository pattern, but direct injection of DBContext into classes
* Add authentication for inter-service communication
* Add Authorization? Investigate Azure AD
* Ocelot or YARP for APIM/reverse-proxy/API gateway
* Something regarding messaging:
	* MassTransit on top of RabbitMQ message broker to try Saga pattern
	* Kafka for event sourcing to try for what it can be used for
		* if decision to move current Unicorn architecture to event-driven will be made, create separate project 'pheonix-project-microservices' 	
* Elasticsearch just to see it in action
* Redis for distributed caching 
* Blazor for some UI and to have something to call API gateway
* Docker support in the form of single command to launch all microservices in containers

## links

* Link to markdown (\*.md) file editor: https://markdown-editor.github.io/
* Link to markdown syntax: https://medium.com/analytics-vidhya/how-to-create-a-readme-md-file-8fb2e8ce24e3
