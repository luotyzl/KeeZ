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
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new SingleView()
            {
                DataContext = new MainViewViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}