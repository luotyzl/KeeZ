﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for LayoutPage.xaml
    /// </summary>
    public partial class LayoutPage : Page
    {
        public LayoutPageViewModel ViewModel { get; } 
		public LayoutPage(LayoutPageViewModel viewModel)
        {
            InitializeComponent();
             ViewModel = viewModel;
            DataContext = this;
       }
    }
}
