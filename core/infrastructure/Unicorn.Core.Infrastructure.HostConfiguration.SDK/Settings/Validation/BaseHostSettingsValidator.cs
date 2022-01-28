using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Validation;

internal static class BaseHostSettingsValidator
{
    public static bool DoesNotContainEmptyStrings(BaseHostSettings settings)
    {
        ValidateAuthenticationSettings(settings.AuthenticationSettings);
        ValidateOneWayCommunicationSettings(settings.OneWayCommunicationSettings);
        ValidateServiceDiscoverySettings(settings.ServiceDiscoverySettings);

        return true;
    }

    private static void ValidateServiceDiscoverySettings(ServiceDiscoverySettings serviceDiscoverySettings)
    {
        if (IsEmptyOrWhiteSpaces(serviceDiscoverySettings.Url))
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(serviceDiscoverySettings.Url)}' is not provided");
        }
    }

    private static void ValidateOneWayCommunicationSettings(OneWayCommunicationSettings oneWayCommunicationSettings)
    {
        if (oneWayCommunicationSettings.SubscriptionId == Guid.Empty)
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(oneWayCommunicationSettings.SubscriptionId)}' is not provided");
        }

        if (IsEmptyOrWhiteSpaces(oneWayCommunicationSettings.ConnectionString))
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(oneWayCommunicationSettings.ConnectionString)}' is not provided");
        }
    }

    private static void ValidateAuthenticationSettings(AuthenticationSettings authenticationSettings)
    {
        if (IsEmptyOrWhiteSpaces(authenticationSettings.AuthorityUrl))
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(authenticationSettings.AuthorityUrl)}' is not provided");
        }

        if (IsEmptyOrWhiteSpaces(authenticationSettings.ClientCredentials.ClientId))
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(authenticationSettings.ClientCredentials.ClientId)}' is not provided");
        }

        if (IsEmptyOrWhiteSpaces(authenticationSettings.ClientCredentials.ClientSecret))
        {
            throw new ArgumentException($"Host settings balue for " +
                $"'{nameof(authenticationSettings.ClientCredentials.ClientSecret)}' is not provided");
        }
    }

    private static bool IsEmptyOrWhiteSpaces(string value) =>
        string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
}
