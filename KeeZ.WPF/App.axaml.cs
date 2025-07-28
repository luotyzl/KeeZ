using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KeeZ.WPF.ViewModels;
using KeeZ.WPF.Views;

namespace KeeZ.WPF;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MvvmSplashWindow()
            {
                DataContext = new SplashViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void TrayMenuItem_Exit_OnClick(object? sender, EventArgs e)
    {
        
    }
}