using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformWellSync.Data;
using PlatformWellSync.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        configuration.GetConnectionString(
            "DefaultConnection"));
});

services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress =
        new Uri("http://test-demo.aemenersol.com");
});

services.AddScoped<SyncService>();

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();

var syncService =
    scope.ServiceProvider
        .GetRequiredService<SyncService>();

await syncService.SyncAsync();

Console.WriteLine("Synchronization completed.");