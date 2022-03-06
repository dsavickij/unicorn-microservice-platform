using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Templates.Service.Features.CreateItem;
using Unicorn.Templates.Service.Features.UpdateAllItemPrices;
using Unicorn.Templates.Service.SDK.DTOs;
using Unicorn.Templates.Service.SDK.Services.Http;

namespace Unicorn.Templates.Service.Host.Controllers;

public class YOUR_SERVICE_NAMEController : UnicornHttpService<IYOUR_SERVICE_NAMEService>, IYOUR_SERVICE_NAMEService
{
    private readonly ILogger<YOUR_SERVICE_NAMEController> _logger;

    public YOUR_SERVICE_NAMEController(ILogger<YOUR_SERVICE_NAMEController> logger) => _logger = logger;

    [HttpPost("api/YOUR_SERVICE_NAME/items")]
    public async Task<OperationResult<Item>> CreateItemAsync([FromBody] Item item)
    {
        return await SendAsync(new CreateItemRequest { Item = item });
    }

    [NonAction]
    public async Task UpdateAllItemPricesAsync()
    {
        await SendAsync(new UpdateAllItemPricesRequest());
    }

    [HttpDelete("api/YOUR_SERVICE_NAME/items/{id}")]
    public Task<OperationResult> DeleteItemAsync([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("api/YOUR_SERVICE_NAME/items/{id}")]
    public Task<OperationResult<Item>> GetItemAsync([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("api/YOUR_SERVICE_NAME/items")]
    public Task<OperationResult<Item>> UpdateItemAsync([FromBody] Item item)
    {
        throw new NotImplementedException();
    }
}
