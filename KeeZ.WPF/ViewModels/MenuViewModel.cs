using System.Collections.ObjectModel;

namespace KeeZ.WPF.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        MenuItems = new ObservableCollection<MenuItemViewModel>
        {
            new() { MenuHeader = "All Items", Key = MenuKeys.MenuKeyAllItems, IsSeparator = false , MenuIconName = "SemiIconList"},

            new() { MenuHeader = "Tags", IsSeparator = true },
            new() { MenuHeader = "lol", IsSeparator = false },

            new() { MenuHeader = "Groups", IsSeparator = true },
            new() { MenuHeader = "Recycle Bin",Key = MenuKeys.MenuKeyRecycleBin,  IsSeparator = false, MenuIconName = "SemiIconArchive"},

            new() { MenuHeader = "Audit", IsSeparator = true },
            new() { MenuHeader = "Duplicate", Key = MenuKeys.MenuKeyAuditDuplicate,  IsSeparator = false , MenuIconName = "SemiIconAlertTriangle"},
            new() { MenuHeader = "Weak", Key = MenuKeys.MenuKeyAuditWeak,  IsSeparator = false , MenuIconName = "SemiIconAlertTriangle"}
        };
    }

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
}

public static class MenuKeys
{
    public const string MenuKeyAllItems = "AllItems";
    public const string MenuKeyRecycleBin = "RecycleBin";
    public const string MenuKeyAuditDuplicate = "AuditDuplicate";
    public const string MenuKeyAuditWeak = "AuditWeak";
}