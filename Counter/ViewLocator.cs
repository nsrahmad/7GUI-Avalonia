using System;
using System.ComponentModel;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace _1_Counter;
public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        string name = data!.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = Type.GetType(name);

        return type != null ? (Control)Activator.CreateInstance(type)! : new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is INotifyPropertyChanged;
}