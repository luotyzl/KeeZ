using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.Avalonia.Core.ViewModels;

public class TabControlDemoViewModel : ObservableObject
{
    public ObservableCollection<string> Items => new(Enumerable.Range(1, 200).Select(a => "Tab " + a));
}