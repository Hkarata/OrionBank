using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args);

builder.UseOrleans((context, siloBuilder) =>
{
    siloBuilder
        .UseLocalhostClustering()
        .ConfigureLogging(logging => logging.AddConsole());
})
    .UseConsoleLifetime();

var app = builder.Build();

await app.RunAsync();