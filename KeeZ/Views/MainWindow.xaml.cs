using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;
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
    

    public INavigationView GetNavigation()
    {
        throw new NotImplementedException();
    }

    public bool Navigate(Type pageType)
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider)
    {
        throw new NotImplementedException();
    }

    void INavigationWindow.ShowWindow()
    {
        ShowWindow();
    }

    public void CloseWindow()
    {
        throw new NotImplementedException();
    }

    private void ShowWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

}