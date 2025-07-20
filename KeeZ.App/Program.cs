using System;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Media;

namespace KeeZ.App;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .With(new FontManagerOptions
        {
            FontFallbacks = new[]
            {
                new FontFallback
                {
                    FontFamily = new FontFamily("Microsoft YaHei")
                }
            }
        })
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<KeeZ.WPF.App>()
            .UseManagedSystemDialogs()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions())
            .LogToTrace();
}