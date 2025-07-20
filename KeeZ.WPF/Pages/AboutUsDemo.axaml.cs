using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KeeZ.WPF.ViewModels;

namespace KeeZ.WPF.Pages;

public partial class AboutUsDemo : UserControl
{
    public AboutUsDemo()
    {
        InitializeComponent();
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (DataContext is AboutUsDemoViewModel vm)
        {
            var launcher = TopLevel.GetTopLevel(this)?.Launcher;
            vm.Launcher = launcher;
        }
    }
}