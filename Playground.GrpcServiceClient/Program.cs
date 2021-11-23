//using GrpcService1.SDK.GrpcClients;

using GrpcService1.SDK.GrpcClients;
using Playground.Common.SDK.Host;

//var builder = PlaygroundHostBuilder2.CreateBuilder(args);

var builder = WebApplication.CreateBuilder(args);

builder.Host.ApplyPlaygroundConfiguration();

//builder.Host.ConfigureHostConfiguration(a => a.)
//var config = builder.Configuration;

//builder.Services.Configure<object>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddGreeterProtoClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
