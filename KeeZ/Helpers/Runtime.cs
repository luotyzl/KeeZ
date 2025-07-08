using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Extensions.Hosting;

namespace KeeZ.Helpers;

internal static class Runtime
{
    public static bool IsDebuggerAttached => Debugger.IsAttached;
    public static bool IsDesignMode { get; } = GetDesignMode();

    public static bool IsDebug =>
#if DEBUG
        true;

#else
        false;
#endif
    
    private static bool GetDesignMode()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return true;
        }
        else if (Process.GetCurrentProcess().ProcessName == "devenv")
        {
            return true;
        }
        return false;
    }
    
    public static void CheckSingleInstance(string instanceName, Action<bool> callback = null!)
    {
        EventWaitHandle? handle;

        try
        {
            handle = EventWaitHandle.OpenExisting(instanceName);
            handle.Set();
            callback?.Invoke(false);
            Environment.Exit(0xFFFF);
        }
        catch (WaitHandleCannotBeOpenedException)
        {
            callback?.Invoke(true);
            handle = new EventWaitHandle(false, EventResetMode.AutoReset, instanceName);
        }

        _ = Task.Factory.StartNew(() =>
        {
            while (handle.WaitOne())
            {
                Application.Current.Dispatcher?.BeginInvoke(() =>
                {
                    Application.Current.MainWindow?.Show();
                    Application.Current.MainWindow?.Activate();
                });
            }
        }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
    }

    
}

internal static class RuntimeExtension
{
    public static IHostBuilder UseSingleInstance(this IHostBuilder self, string instanceName, Action<bool> callback = null!)
    {
        if (!Environment.GetCommandLineArgs().Contains("--no-single"))
        {
            Runtime.CheckSingleInstance(instanceName, callback);
        }
        return self;
    }
}
