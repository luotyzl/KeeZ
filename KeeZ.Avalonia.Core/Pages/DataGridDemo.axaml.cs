using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class DataGridDemo : UserControl
{
    public DataGridDemo()
    {
        InitializeComponent();
        DataContext = new DataGridDemoViewModel();
    }
}