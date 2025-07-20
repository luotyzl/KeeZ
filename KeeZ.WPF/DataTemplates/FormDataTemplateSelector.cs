using Avalonia.Controls;
using Avalonia.Controls.Templates;
using KeeZ.WPF.ViewModels;

namespace KeeZ.WPF.Converters;

public class FormDataTemplateSelector: ResourceDictionary, IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null) return null;
        var type = param.GetType();
        if (this.TryGetResource(type, null, out var template) && template is IDataTemplate dataTemplate)
        {
            return dataTemplate.Build(param);
        }
        return null;
    }

    public bool Match(object? data)
    {
        return data is IFromItemViewModel;
    }
}