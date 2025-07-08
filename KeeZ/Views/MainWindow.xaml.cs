using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Wpf.Ui;
using Application = System.Windows.Application;

namespace KeeZ;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
    public MainWindow()
    {
        InitializeComponent();
        // InitializeWebView();
        InitializeSystemTray();
    }
    
    // private async void InitializeWebView()
    // {
    //     try
    //     {
    //         await Browser.EnsureCoreWebView2Async(null);
    //     }
    //     catch
    //     {
    //         //ignore
    //     }
    // }
    
    private NotifyIcon _notifyIcon;
    private void InitializeSystemTray()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon("favicon.ico"), // Your icon file
            Text = "KeeZ",
            Visible = true
        };
        
        // Add context menu
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Show", null, (s, e) => ShowWindow());
        contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
        _notifyIcon.ContextMenuStrip = contextMenu;
        
        // Double click to show window
        _notifyIcon.DoubleClick += (s, e) => ShowWindow();
        
        // Handle window closing to minimize to tray instead
        Closing += (s, e) =>
        {
            e.Cancel = true;
            HideWindow();
        };
    }
    private void HideWindow()
    {
        Hide();
    }
    private void ExitApplication()
    {
        _notifyIcon.Dispose();
        Application.Current.Shutdown();
    }
    private void ShowWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }
    protected override void OnClosed(EventArgs e)
    {
        _notifyIcon?.Dispose();
        base.OnClosed(e);
    }
}