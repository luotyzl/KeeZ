using System.Configuration;
using System.Data;
using System.Windows;
using KeeZ.Common;
using KeeZ.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace KeeZ;
public partial class App : Application
{
    public static IHost AppHost { get; private set; }
    public static IServiceProvider Services { get; set; }
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .UseSerilog((ctx, lc, configuration) =>
            {
                configuration
                    .WriteTo.Console()
                    .MinimumLevel.Debug()
                    .WriteTo
                    .File(
                        "Logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7,
                        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    );
            })
            .ConfigureServices((ctx, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
    }
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        
            
        var logger = Services.GetRequiredService<ILogger<App>>();
        var _ = ConfigManager.LoadSettings<UserSettings>();
        logger.LogInformation("Application started.");
        // var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        // mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); // Remove default providers
            loggingBuilder.AddSerilog();     // Use Serilog
        });
        Services = services.BuildServiceProvider();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); // Remove default providers
            loggingBuilder.AddSerilog();     // Use Serilog
        });
        // Register your services here
        // services.AddSingleton<KeepassDbContext>();
        Services = services.BuildServiceProvider();
    }
}