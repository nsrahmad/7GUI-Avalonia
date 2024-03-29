using _1_Counter.ViewModels;
using _1_Counter.Views;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotAvalonia;

namespace _1_Counter;
public partial class App : Application
{
    public override void Initialize() {
        this.EnableHotReload();
        AvaloniaXamlLoader.Load(this);
    } 

    public override void OnFrameworkInitializationCompleted()
    {
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