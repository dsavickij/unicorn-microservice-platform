using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;

public static class RestServiceEndpointMapper
{
    public static void MapUnicornRestService<TService>(this IEndpointRouteBuilder builder)
        where TService : class
    {
        if (!typeof(TService).IsInterface)
        {
            throw new ArgumentException($"Generic parameter {typeof(TService).Name} is not interface");
        }

        if (typeof(TService).GetCustomAttribute<UnicornRestServiceMarkerAttribute>() is null)
        {
            throw new ArgumentException($"Generic parameter {typeof(TService).Name} must be decorated with " +
                                        $"{nameof(UnicornRestServiceMarkerAttribute)} attribute");
        }

        var restServiceInterfaceMethods = typeof(TService).GetMethods();

        if (restServiceInterfaceMethods.Length == 0)
        {
            return;
        }

        foreach (var restMethod in restServiceInterfaceMethods)
        {
            var attributes = restMethod.GetCustomAttributes(true);

            foreach (var attribute in attributes.Where(x => x is HttpMethodAttribute))
            {
                _ = attribute switch
                {
                    GetAttribute get => MapGet<TService>(builder, restMethod!, get),
                    PostAttribute post => MapPost<TService>(builder, restMethod!, post),
                    PutAttribute put => MapPut<TService>(builder, restMethod, put),
                    DeleteAttribute delete => MapDelete<TService>(builder, restMethod, delete),
                    PatchAttribute patch => throw new NotSupportedException(
                        $"Rest service attribute ${nameof(PatchAttribute)} is not supported"),
                    MultipartAttribute multipart => throw new NotSupportedException(
                        $"Rest service attribute ${nameof(MultipartAttribute)} is not supported"),
                    _ => throw new NotSupportedException($"Attribute {attribute.GetType().Name} is not supported")
                };
            }
        }
    }

    private static int MapDelete<TService>(
        IEndpointRouteBuilder builder,
        MethodInfo serviceInterfaceMethod,
        DeleteAttribute deleteAttribute)
        where TService : class
    {
        builder.MapDelete(deleteAttribute.Path, GetDelegate<TService>(builder, serviceInterfaceMethod)).WithOpenApi();

        return 1;
    }

    private static int MapPut<TService>(
        IEndpointRouteBuilder builder,
        MethodInfo serviceInterfaceMethod,
        PutAttribute putAttribute)
        where TService : class
    {
        builder.MapPut(putAttribute.Path, GetDelegate<TService>(builder, serviceInterfaceMethod)).WithOpenApi();

        return 1;
    }

    private static int MapPost<TService>(
        IEndpointRouteBuilder builder,
        MethodInfo serviceInterfaceMethod,
        PostAttribute postAttribute)
        where TService : class
    {
        var endpointBuilder = builder
            .MapPost(postAttribute.Path, GetDelegate<TService>(builder, serviceInterfaceMethod))
            .WithOpenApi();

        // error if antiforgery is not disabled for file upload
        var pars = serviceInterfaceMethod.GetParameters();

        if (pars.Any(x => x.ParameterType == typeof(IFormFile)))
        {
            endpointBuilder.DisableAntiforgery();
        }

        return 1;
    }

    private static int MapGet<TService>(IEndpointRouteBuilder builder, MethodInfo serviceInterfaceMethod,
        GetAttribute getAttribute) where TService : class
    {
        builder.MapGet(getAttribute.Path, GetDelegate<TService>(builder, serviceInterfaceMethod)).WithOpenApi();

        return 1;
    }

    private static Delegate GetDelegate<TService>(IEndpointRouteBuilder builder, MethodInfo serviceInterfaceMethod)
        where TService : class
    {
        var service = ResolveUnicornRestService<TService>(builder.ServiceProvider);
        var serviceMethod = GetUnicornRestServiceMethod(service, serviceInterfaceMethod);
        return GetRestEndpointDelegate(service, serviceMethod);
    }

    private static Delegate GetRestEndpointDelegate<TService>(TService service, MethodInfo serviceMethod)
    {
        var tArgs = new List<System.Type>();
        var serviceMethodPars = serviceMethod.GetParameters();

        // TODO: I probably need to exclude file upload over Refit service with some attribute. Some testing need to be done
        if (serviceMethodPars.Any(x => x.ParameterType == typeof(IFormFile)) && serviceMethodPars.Length > 1)
        {
            var nonFormFilePars = serviceMethodPars.Where(x => x.ParameterType != typeof(IFormFile));

            foreach (ParameterInfo nonFormFilePar in nonFormFilePars)
            {
                if (nonFormFilePar.CustomAttributes.All(x => x.AttributeType != typeof(FromFormAttribute)))
                {
                    throw new InvalidOperationException(
                        $"Method with parameter type of {nameof(IFormFile)} must have parameter {nonFormFilePar.Name} " +
                        $"decorated with {nameof(FromFormAttribute)} attribute");
                }
            }
        }

        foreach (var parameter in serviceMethod.GetParameters())
        {
            tArgs.Add(parameter.ParameterType);
        }

        tArgs.Add(serviceMethod.ReturnType);
        var delegateType = Expression.GetDelegateType([.. tArgs]);

        return serviceMethod.CreateDelegate(delegateType, service);
    }

    private static MethodInfo GetUnicornRestServiceMethod<TService>(TService resolvedService,
        MethodInfo serviceInterfaceMethod) where TService : class
    {
        var serviceType = resolvedService.GetType();
        var serviceMethods = serviceType.GetMethods()
                                 .Where(x => x.Name == serviceInterfaceMethod.Name)
                                 .ToList()
                             ?? throw new ArgumentNullException($"No method by name ${serviceInterfaceMethod.Name} " +
                                                                $"was found in service implementation ${serviceType.Name}");

        MethodInfo serviceMethod;

        if (serviceMethods.Count() > 1)
        {
            var serviceInterfaceMethodParams = serviceInterfaceMethod.GetParameters();
            serviceMethod =
                serviceMethods.Single(x => x.GetParameters().All(p => serviceInterfaceMethodParams.Contains(p)));
        }
        else
        {
            serviceMethod = serviceMethods.Single();
        }

        return serviceMethod;
    }

    private static TService ResolveUnicornRestService<TService>(IServiceProvider provider)
        where TService : class
    {
        var services = provider.GetServices<TService>();

        var service = services.FirstOrDefault(x => x.GetType() != typeof(IInterceptor));

        return service ?? throw new ArgumentNullException(
            $"Could not resolve a service of type {typeof(TService).Name}. " +
            $"Make sure it is registered in dependency injection container");
    }
}