using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;

namespace KeeZ.Avalonia.Core.Pages;

public partial class ListBoxDemo : UserControl
{
    public ListBoxDemo()
    {
        InitializeComponent();
    }

    public IEnumerable Items { get; set; } = new List<string> { "Ding", "Otter", "Husky", "Mr.17", "Cass", };
}