using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.WPF.ViewModels;

public partial class TimePickerDemoViewModel: ObservableObject
{
    [ObservableProperty] private TimeSpan? _time;
    
    public TimePickerDemoViewModel()
    {
        Time = new TimeSpan(12, 20, 0);
    }
}