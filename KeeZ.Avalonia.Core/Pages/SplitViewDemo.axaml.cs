using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class SplitViewDemo : UserControl
{
    public SplitViewDemo()
    {
        InitializeComponent();
        this.DataContext = new SplitViewDemoViewModel();
    }
}