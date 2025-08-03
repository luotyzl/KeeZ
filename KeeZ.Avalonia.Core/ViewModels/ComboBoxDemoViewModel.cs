using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.Avalonia.Core.ViewModels;

public class ComboBoxDemoViewModel : ObservableObject
{
    public ObservableCollection<string> Items { get; set; } = ["Ding", "Otter", "Husky", "Mr.17", "Cass"];
}