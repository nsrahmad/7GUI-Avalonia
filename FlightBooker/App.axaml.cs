using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using static Avalonia.Data.Core.Plugins.BindingPlugins;

using FlightBooker.ViewModels;
using FlightBooker.Views;

namespace FlightBooker;
public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        for(var plugin = 0; plugin < DataValidators.Count; plugin++)
        {
            DataValidators.RemoveAt(plugin);
        }
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}