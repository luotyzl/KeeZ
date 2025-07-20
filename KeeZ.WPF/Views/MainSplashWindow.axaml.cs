using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KeeZ.WPF.ViewModels;
using Ursa.Controls;

namespace KeeZ.WPF.Views;

public partial class MainSplashWindow : SplashWindow
{
    public MainSplashWindow()
    {
        InitializeComponent();
    }

    protected override async Task<Window?> CreateNextWindow()
    {
        return new MainWindow()
        {
            DataContext = new MainViewViewModel()
        };
    }
}