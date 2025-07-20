using Avalonia;
using Avalonia.Controls;
using KeeZ.WPF.ViewModels;
using Ursa.Controls;

namespace KeeZ.WPF.Pages;

public partial class PopConfirmDemo : UserControl
{
    public PopConfirmDemo()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not PopConfirmDemoViewModel vm) return;
        var topLevel = TopLevel.GetTopLevel(this);
        vm.ToastManager = WindowToastManager.TryGetToastManager(topLevel, out var manager)
            ? manager
            : new WindowToastManager(topLevel);
    }
}