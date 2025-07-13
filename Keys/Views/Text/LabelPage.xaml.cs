﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for LabelPage.xaml
    /// </summary>
    public partial class LabelPage : Page
    {
        public LabelPageViewModel ViewModel { get; }
public LabelPage(LabelPageViewModel viewModel)
        {
ViewModel = viewModel;
DataContext = this;
            InitializeComponent();
        }
    }
}
