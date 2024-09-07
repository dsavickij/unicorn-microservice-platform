using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateGrpcServiceConfiguration;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateGrpcServiceConfiguration.Validation;

public class UpdateGrpcServiceConfigurationRequestValidator : AbstractValidator<UpdateGrpcServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;

    public UpdateGrpcServiceConfigurationRequestValidator(ServiceDiscoveryDbContext context)
    {
        _ctx = context;

        RegisterRules();
    }

    private void RegisterRules()
    {
        RuleFor(x => x.Configuration.ServiceHostName)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage(x => $"'{nameof(x.Configuration.ServiceHostName)}' is not provided")
            .CustomAsync(async (serviceHostName, validationCtx, ct) =>
            {
                var result = await _ctx.GrpcServiceConfigurations.SingleOrDefaultAsync(x => x.ServiceHostName == serviceHostName);

                if (result is null)
                {
                    var failure = new ValidationFailure(nameof(UpdateGrpcServiceConfigurationRequest.Configuration.ServiceHostName),
                        $"gRPC service configuration with service host name '{serviceHostName}' does not exist");

                    validationCtx.AddFailure(failure);
                }
            });

        RuleFor(x => x.Configuration.BaseUrl)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage(x => $"'{nameof(x.Configuration.BaseUrl)}' is not provided")
            .Custom((serviceBaseUrl, validationCtx) =>
            {
                if (Uri.TryCreate(serviceBaseUrl, UriKind.Absolute, out var uri) is false)
                {
                    var failure = new ValidationFailure(nameof(UpdateGrpcServiceConfigurationRequest.Configuration.BaseUrl),
                        $"Url '{serviceBaseUrl}' is not valid");

                    validationCtx.AddFailure(failure);
                }
            });
    }
}
