﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for MediaPage.xaml
    /// </summary>
    public partial class MediaPage : Page
    {
        public MediaPageViewModel ViewModel { get; } 
		public MediaPage(MediaPageViewModel viewModel)
        {
            InitializeComponent();
             ViewModel = viewModel;
            DataContext = this;
       }
    }
}
