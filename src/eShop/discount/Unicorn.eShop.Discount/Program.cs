using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.eShop.Discount;
using Unicorn.eShop.Discount.Services.gRPC;

ServiceHostBuilder.Build<DiscountHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, _, _) =>
    {
        services.AddGrpc();
    })
    .WithEndpointConfiguration(endpointBuilder =>
    {
        endpointBuilder.MapGrpcService<DiscountGrpcService>();
    });
}).Run();
