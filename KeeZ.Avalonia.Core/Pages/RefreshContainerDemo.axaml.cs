using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class RefreshContainerDemo : UserControl
{
    private RefreshContainerDemoViewModel _viewModel;

    public RefreshContainerDemo()
    {
        InitializeComponent();
        _viewModel = new RefreshContainerDemoViewModel();
        DataContext = _viewModel;
    }

    private async void RefreshContainerPage_RefreshRequested(object? sender, RefreshRequestedEventArgs e)
    {
        var deferral = e.GetDeferral();
        await _viewModel.AddToTop();
        deferral.Complete();
    }
}