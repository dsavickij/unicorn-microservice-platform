# Service setup

## Program.cs configuration

1. Open `Program.cs` in your project.
2. Delete all the code in this class.
3. Paste the following code in `Program.cs`:

```c#

using Unicorn.Core.Infrastructure.Host.SDK;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Host.ApplyUnicornConfiguration<YourServiceSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();

```

4. Substitue `YourServiceSettings` with the settings class/record for your service. This class/record must  inherit from `BaseHostSettings`.
5. If needed, add other services and middlewares where comments in pasted code indicates.

## AppSettings.json configuration

1. Open AppSettings.json
2. Delete all the configuration text
3. Paste the following configuration text:

```json

{
  "ApplicationInsights": {
    "InstrumentationKey": "<ADD AZURE APPINSIGHT INSTRUMENTATION KEY IF YOU WANT TO LOG IN AZURE -- OR LEAVE BLANK OTERWISE>"
  },
  "Logging": {
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Trace"
      }
    },
    "LogLevel": {
      "Default": "Trace",
      "Microsoft.AspNetCore": "Trace"
    }
  },
  "<INSERT THE NAME OF YOUR SERVICE SETTINGS CLASS/RECORD>": {
    "ServiceDiscoverySettings": {
      "Url": "<ADD SERVICE DISCOVERY URL>"
    },
    "OneWayCommunicationSettings": {
      "ConnectionString": "<ADD MESSAGE BROKER CONNECTION STRING>",
      "SubscriptionId": "<ADD GUID TO USE FOR SUBSCRIPTIONS TO MESSAGE BROKER TOPICS/EXCHANGE QUEUES>"
    },
    "AuthenticationSettings": {
      "AuthorityUrl": "<ADD AUTHENTICATION SERVICE URL>",
      "ClientCredentials": {
        "ClientId": "<ADD CLIENTID CREATED SPECIFICALLY FOR YOUR SERVICE>",
        "ClientSecret": "<ADD CLIENT SECRET CREATED SPECIFICALLY FOR YOUR SERVICE>",
        "Scopes": "<ADD SCOPES SEPARATED BY SPACE OR LEAVE BLANK>"
      }
    }
  }
}

```