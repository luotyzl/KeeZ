﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for TypographyPage.xaml
    /// </summary>
    public partial class TypographyPage : Page
    {
        public TypographyPage(TypographyPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        public TypographyPageViewModel ViewModel { get; }
    }
}
