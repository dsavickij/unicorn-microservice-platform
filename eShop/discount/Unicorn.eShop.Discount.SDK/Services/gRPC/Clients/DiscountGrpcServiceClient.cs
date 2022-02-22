using Grpc.Core;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;
using Unicorn.eShop.Discount.SDK.DTOs;
using Unicorn.eShop.Discount.SDK.Protos;

namespace Unicorn.eShop.Discount.SDK.Services.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface IDiscountGrpcServiceClient
{
    public Task<OperationResult<CartDiscount>> GetCartDiscountAsync(string discountCode);
}

public class DiscountGrpcServiceClient : BaseGrpcClient, IDiscountGrpcServiceClient
{
    private readonly ILogger<DiscountGrpcServiceClient> _logger;

    public DiscountGrpcServiceClient(IGrpcServiceClientFactory factory, ILogger<DiscountGrpcServiceClient> logger)
        : base(factory)
    {
        _logger = logger;
    }

    public async Task<OperationResult<CartDiscount>> GetCartDiscountAsync(string discountCode)
    {
        try
        {
            var req = new CartDiscountRequest { DiscountCode = discountCode };
            var response = await Factory.CallAsync(
                c => new DiscountGrpcServiceProto.DiscountGrpcServiceProtoClient(c).GetCartDiscountAsyncAsync(req));

            var result = new CartDiscount
            {
                DiscountId = Guid.Parse(response.DiscountId),
                DiscountCode = response.DiscountCode,
                Title = response.Title,
                Description = response.Description,
                DiscountPercentage = response.DiscountPercentage,
            };

            return Ok(result);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return NotFound($"Discount by discountCode '{discountCode}' was not found");
        }
        catch (RpcException ex)
        {
            _logger?.LogError($"Error occured calling gRPC service", ex);
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error occured calling gRPC service", ex);
            throw;
        }
    }
}
