﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;


namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for TabControlPage.xaml
    /// </summary>
    public partial class TabControlPage : Page
    {
    public TabControlPageViewModel ViewModel { get; }

    public TabControlPage(TabControlPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
    }
}
