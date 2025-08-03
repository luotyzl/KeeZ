using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class TabStripDemo : UserControl
{
    public TabStripDemo()
    {
        InitializeComponent();
        this.DataContext = new TabStripDemoViewModel();
    }
}