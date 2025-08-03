using System.Threading.Tasks;
using Avalonia.Controls;
using KeeZ.Avalonia.Core.ViewModels;

namespace KeeZ.Avalonia.Core.Pages;

public partial class VariablesDemo : UserControl
{
    public VariablesDemo()
    {
        InitializeComponent();
        this.DataContext = new VariablesDemoViewModel();
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