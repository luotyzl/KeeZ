﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KeeZ.WPF.ViewModels;

public partial class AvatarDemoViewModel : ViewModelBase
{
    [ObservableProperty] private string _content = "AS";
    [ObservableProperty] private bool _canClick = true;

    [RelayCommand(CanExecute = nameof(CanClick))]
    private void Click()
    {
        Content = "BM";
    }
}