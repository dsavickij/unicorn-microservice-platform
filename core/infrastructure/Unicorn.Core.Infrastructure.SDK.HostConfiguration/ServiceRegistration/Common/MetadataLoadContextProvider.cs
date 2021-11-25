using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.Common;

internal class MetadataLoadContextProvider
{
    public static MetadataLoadContext GetMedataLoadContext()
    {
        //Important: all assemblies (besides current and core assemblies) of referenced projects must be added or exception
        //will be thrown about not loaded assembly. Right now only 2 projects are referenced:
        //Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc and
        //Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http

        var currentAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(object).Assembly;
        var httpServiceMarkerAssembly = typeof(UnicornHttpServiceMarker).Assembly;
        var grpcClientMarkerAssembly = typeof(UnicornGrpcClientMarker).Assembly;

        var filePathes = new[]
        {
            currentAssembly.Location,
            coreAssembly.Location,
            httpServiceMarkerAssembly.Location,
            grpcClientMarkerAssembly.Location
        };

        return new MetadataLoadContext(new PathAssemblyResolver(filePathes), coreAssembly.GetName().Name);
    }
}