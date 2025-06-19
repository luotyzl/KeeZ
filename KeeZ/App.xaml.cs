using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace KeeZ;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost AppHost { get; private set; }
    
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File("logs/keez.log", rollingInterval: RollingInterval.Day))
            .ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        // var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        // mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}