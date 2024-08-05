using OrionBank.Silo.StartUpTasks;
using Orleans.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((context, siloBuilder) =>
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
            dashBoardOptions.HostSelf = false;
        })
        .ConfigureLogging(logging => logging.AddConsole());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Map("", x => x.UseOrleansDashboard());

app.Run();