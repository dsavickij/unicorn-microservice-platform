using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Templates.Service.Features.UpdateAllItemPrices;

public class UpdateAllItemPricesRequestHandler : BaseHandler.WithoutResult.For<UpdateAllItemPricesRequest>
{
    protected override Task HandleAsync(UpdateAllItemPricesRequest request, CancellationToken cancellationToken)
    {
        // do your magic

        return Task.CompletedTask;
    }
}
