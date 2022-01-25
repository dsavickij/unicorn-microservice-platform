using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.PipelineBehaviours;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR;

internal static class MediatorRegistrationExtensions
{
    public static void AddMediatorComponents(this IServiceCollection services)
    {
        AddMediatorRequestsAndHandlers(services);
        AddPipelineBehaviours(services);
        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        AssemblyScanner.UseHostUnicornAssemblies(assemblies => services.AddValidatorsFromAssemblies(assemblies));
    }

    private static void AddPipelineBehaviours(IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }

    private static void AddMediatorRequestsAndHandlers(IServiceCollection services)
    {
        AssemblyScanner.UseHostUnicornAssemblies(assemblies => services.AddMediatR(assemblies));
    }
}
