using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.Features.CreateHttpServiceConfiguration;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.CreateHttpServiceConfiguration.Validation;

public class CreateHttpServiceConfigurationRequestValidator : AbstractValidator<CreateHttpServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;

    public CreateHttpServiceConfigurationRequestValidator(ServiceDiscoveryDbContext context)
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
                var result = await _ctx.HttpServiceConfigurations.SingleOrDefaultAsync(x => x.ServiceHostName == serviceHostName);

                if (result is not null)
                {
                    var failure = new ValidationFailure(nameof(CreateHttpServiceConfigurationRequest.Configuration.ServiceHostName),
                        $"HTTP service configuration with service host name '{serviceHostName}' already exists");

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
                    var failure = new ValidationFailure(nameof(CreateHttpServiceConfigurationRequest.Configuration.BaseUrl),
                        $"Url '{serviceBaseUrl}' is not valid");

                    validationCtx.AddFailure(failure);
                }
            });
    }
}
