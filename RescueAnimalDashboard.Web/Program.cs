using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RescueAnimalDashboard.Web.Components;
using RescueAnimalDashboard.Web.Services.Api;
using RescueAnimalDashboard.Web.Services.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root components
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add MudBlazor
builder.Services.AddMudServices();

// Add API service (URL comes from wwwroot/appsettings*.json)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    throw new InvalidOperationException("ApiBaseUrl is missing from wwwroot/appsettings.json.");
}

builder.Services.AddHttpClient<IAnimalApiClient, AnimalApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Add state
builder.Services.AddSingleton<DashboardState>();

var host = builder.Build();

// Start loading data immediately. Dashboard awaits the same task.
_ = host.Services.GetRequiredService<DashboardState>().LoadAsync();

await host.RunAsync();
