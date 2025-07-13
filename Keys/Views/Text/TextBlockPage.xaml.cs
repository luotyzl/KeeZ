﻿using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for TextBlockPage.xaml
    /// </summary>
    public partial class TextBlockPage : Page
    {
        public TextBlockPageViewModel ViewModel { get; }
public TextBlockPage(TextBlockPageViewModel viewModel)
        {
ViewModel = viewModel;
DataContext = this;
            InitializeComponent();
        }
    }
}
