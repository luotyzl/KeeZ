using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.WPF.ViewModels;

public partial class DateRangePickerDemoViewModel: ObservableObject
{
    [ObservableProperty] private DateTime? _startDate;
    [ObservableProperty] private DateTime? _endDate;

    public DateRangePickerDemoViewModel()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);
    }
}