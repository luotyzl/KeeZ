using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.Avalonia.Core.ViewModels;

public class TabStripDemoViewModel : ObservableObject
{
    public ObservableCollection<string> Items => new(Enumerable.Range(1, 10).Select(a => "Tab " + a));
}