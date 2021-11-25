using Playground.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http;
using System.Reflection;

namespace Playground.Core.Infrastructure.SDK.HostConfiguration.Common;

internal class MetadataLoadContextProvider
{
    public static MetadataLoadContext GetMedataLoadContext()
    {
        //Important: all assemblies (besides current and core assemblies) of referenced projects must be added or exception
        //will be thrown about not loaded assembly. Right now only 2 projects are referenced:
        //Playground.Core.Infrastructure.SDK.ServiceCommunication.Grpc and
        //Playground.Core.Infrastructure.SDK.ServiceCommunication.Http

        var currentAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(object).Assembly;
        var httpServiceMarkerAssembly = typeof(PlaygroundHttpServiceMarker).Assembly;
        var grpcServiceMarkerAssembly = typeof(PlaygroundGrpcClientMarker).Assembly;

        var filePathes = new[]
        {
            currentAssembly.Location,
            coreAssembly.Location,
            httpServiceMarkerAssembly.Location,
            grpcServiceMarkerAssembly.Location
        };

        return new MetadataLoadContext(new PathAssemblyResolver(filePathes), coreAssembly.GetName().Name);
    }
}