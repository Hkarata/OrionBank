var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();

//builder.Host.UseOrleansClient(clientBuilder =>
//{
//    clientBuilder.UseLocalhostClustering()
//                    .Configure<ClusterOptions>(clusterOptions =>
//                    {
//                        clusterOptions.ClusterId = "dev";
//                        clusterOptions.ServiceId = "OrionBank";
//                    });
//});

builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton<BaseClusterService>();
//builder.Services.AddSingleton<CustomerManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
