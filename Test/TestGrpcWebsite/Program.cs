using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 9300, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        //listenOptions.UseHttps("<path to .pfx file>", "<certificate password>");
    });
});
var app = builder.Build();

app.MapGrpcService<GreeterService>();

app.MapGet("/", () => "Hello World!");

app.Run();