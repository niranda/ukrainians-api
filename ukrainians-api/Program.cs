using NomadChat.WebAPI;
using NomadChat.WebAPI.Helpers.Database;

var host = CreateHostBuilder(args).Build();
await InitDatabase(host);
await host.RunAsync();

static async Task InitDatabase(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await DbInitializer.Initialize(services);
    }
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}