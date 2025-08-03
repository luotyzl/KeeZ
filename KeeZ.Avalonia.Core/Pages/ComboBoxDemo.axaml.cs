using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class ComboBoxDemo : UserControl
{
    public ComboBoxDemo()
    {
        InitializeComponent();
        this.DataContext = new ComboBoxDemoViewModel();
    }
}