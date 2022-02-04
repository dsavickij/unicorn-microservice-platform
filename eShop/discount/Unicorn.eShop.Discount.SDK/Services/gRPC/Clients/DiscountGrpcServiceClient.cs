using DiscountGrpcServiceProto;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.eShop.Discount.SDK.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface IDiscountGrpcServiceClient
{
    public Task<OperationResult<CartDiscountDTO>> GetCartDiscountAsync(string discountCode);
}

public class DiscountGrpcServiceClient : BaseGrpcClient, IDiscountGrpcServiceClient
{
    private DiscountGrpcServiceProto.DiscountGrpcServiceProto.DiscountGrpcServiceProtoClient? _client;
    private readonly ILogger<DiscountGrpcServiceClient> _logger;

    protected override string GrpcServiceName => "DiscountGrpcService";

    public DiscountGrpcServiceClient(IGrpcServiceClientFactory factory, ILogger<DiscountGrpcServiceClient> logger) : base(factory)
    {
        _logger = logger;
    }

    public async Task<OperationResult<CartDiscountDTO>> GetCartDiscountAsync(string discountCode)
    {
        try
        {
            var req = new CartDiscountRequest { DiscountCode = discountCode };
            var response = await Factory.CallAsync(GrpcServiceName, c => GetClient(c).GetCartDiscountAsyncAsync(req));

            var result = new CartDiscountDTO
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
            _logger?.LogError($"Error occured calling gRPC service '{GrpcServiceName}'", ex);
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error occured calling gRPC service '{GrpcServiceName}'", ex);
            throw;
        }
    }

    private DiscountGrpcServiceProto.DiscountGrpcServiceProto.DiscountGrpcServiceProtoClient GetClient(GrpcChannel channel) =>
        _client ??= new DiscountGrpcServiceProto.DiscountGrpcServiceProto.DiscountGrpcServiceProtoClient(channel);
}
