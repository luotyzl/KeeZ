using System.Collections.Generic;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.WPF.ViewModels;

public class KeyGestureInputDemoViewModel: ObservableObject
{
    public List<Key> AcceptableKeys { get; set; } = new List<Key>()
    {
        Key.A, Key.B, Key.C,
    };
}