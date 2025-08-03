using CommunityToolkit.Mvvm.ComponentModel;

namespace KeeZ.Avalonia.Core.ViewModels;

public class TreeDataGridDemoViewModel : ObservableObject
{
    public SongsPageViewModel SongsContext { get; } = new();
    public FilesPageViewModel FilesContext { get; } = new();
}