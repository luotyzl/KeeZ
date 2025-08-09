using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class Passwords : UserControl
{
    public Passwords()
    {
        InitializeComponent();
        this.DataContext = new PasswordsViewModel();
    }

    protected override async void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PasswordsViewModel? vm = this.DataContext as PasswordsViewModel;
        await Dispatcher.UIThread.InvokeAsync(() => { vm?.InitializeResources(); });
    }

    public async Task Copy(object? o)
    {
        if (o is null) return;
        var toplevel = TopLevel.GetTopLevel(this);
        if (toplevel?.Clipboard is { } c)
        {
            await c.SetTextAsync(o.ToString());
        }
    }
}