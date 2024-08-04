using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrionBank.Silo.StartUpTasks;


var builder = Host.CreateDefaultBuilder(args);

builder.UseOrleans((context, siloBuilder) =>
{
    siloBuilder
        .UseLocalhostClustering()
        .AddAdoNetGrainStorage("OrionBank", options =>
        {
            options.Invariant = "System.Data.SqlClient";
            options.ConnectionString = "Data Source=HERIS_PC\\SQLEXPRESS;Database=OrionBankDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";
        })
        .UseTransactions()
        .AddStartupTask<SeedCustomersTask>()
        .ConfigureLogging(logging => logging.AddConsole());
})
    .UseConsoleLifetime();

var app = builder.Build();

await app.RunAsync();