using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class AutoCompleteBoxDemo : UserControl
{
    public AutoCompleteBoxDemo()
    {
        InitializeComponent();
        this.DataContext = new AutoCompleteBoxDemoViewModel();
    }
}