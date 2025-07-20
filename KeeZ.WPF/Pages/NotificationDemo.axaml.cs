using Avalonia;
using Avalonia.Controls;
using KeeZ.WPF.ViewModels;
using Ursa.Controls;

namespace KeeZ.WPF.Pages;

public partial class NotificationDemo : UserControl
{
    public NotificationDemo()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not NotificationDemoViewModel vm) return;
        var topLevel = TopLevel.GetTopLevel(this);
        vm.NotificationManager = WindowNotificationManager.TryGetNotificationManager(topLevel, out var manager)
            ? manager
            : new WindowNotificationManager(topLevel);
    }
}