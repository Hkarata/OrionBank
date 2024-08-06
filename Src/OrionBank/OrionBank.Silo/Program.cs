using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrionBank.Silo.StartUpTasks;
using Orleans.Configuration;


var builder = Host.CreateDefaultBuilder(args);

builder.UseOrleans((context, siloBuilder) =>
{
    siloBuilder
        .UseLocalhostClustering()
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "OrionBank";
        })
        .AddAdoNetGrainStorage("OrionBank", options =>
        {
            options.Invariant = "Microsoft.Data.SqlClient";
            options.ConnectionString = "Data Source=HERIS_PC\\SQLEXPRESS;Database=OrionBankDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True";
        })
        .UseTransactions()
        //.AddStartupTask<SeedCustomersTask>()
        .UseDashboard(dashBoardOptions =>
        {
            dashBoardOptions.Username = "admin";
            dashBoardOptions.Password = "admin";
            dashBoardOptions.HostSelf = true;
            dashBoardOptions.Host = "*";
            dashBoardOptions.Port = 8080;
        })
        .ConfigureLogging(logging => logging.AddConsole());
});

var app = builder.Build();

app.Run();