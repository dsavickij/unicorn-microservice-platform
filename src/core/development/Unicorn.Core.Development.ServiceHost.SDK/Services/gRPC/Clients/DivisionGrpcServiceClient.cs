﻿using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface IDivisionGrpcServiceClient
{
    Task<double> DivideAsync(int first, int second);
}

public class DivisionGrpcServiceClient : BaseGrpcClient, IDivisionGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public DivisionGrpcServiceClient(IGrpcServiceClientFactory factory) : base(factory)
    {
        _factory = factory;
    }

    public async Task<double> DivideAsync(int first, int second)
    {
        var response = await _factory.CallAsync(
            c => new DivisionGrpcService.DivisionGrpcServiceClient(c).DivideAsync(
                new DivisionRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }
}
