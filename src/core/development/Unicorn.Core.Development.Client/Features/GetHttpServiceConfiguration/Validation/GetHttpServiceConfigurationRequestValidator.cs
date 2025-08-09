using FluentValidation;

namespace Unicorn.Core.Development.Client.Features.GetHttpServiceConfiguration.Validation;

public class GetHttpServiceConfigurationRequestValidator : AbstractValidator<GetHttpServiceConfigurationRequest>
{
    public GetHttpServiceConfigurationRequestValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage(x => $"'{nameof(x.ServiceName)}' is not provided");
    }
}
