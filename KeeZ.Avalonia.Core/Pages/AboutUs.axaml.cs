using Avalonia.Controls;
using Avalonia.Interactivity;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class AboutUs : UserControl
{
    public AboutUs()
    {
        InitializeComponent();
        this.DataContext = new AboutUsViewModel();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (this.DataContext is AboutUsViewModel vm)
        {
            var launcher = TopLevel.GetTopLevel(this)?.Launcher;
            vm.Launcher = launcher;
        }
    }
}