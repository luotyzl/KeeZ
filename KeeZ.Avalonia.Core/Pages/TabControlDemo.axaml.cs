using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class TabControlDemo : UserControl
{
    public TabControlDemo()
    {
        InitializeComponent();
        this.DataContext = new TabControlDemoViewModel();
    }
}