using System;

using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Timer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly DispatcherTimer aTimer;

    public MainWindowViewModel()
    {
        aTimer = new(new TimeSpan(0, 0, 1), DispatcherPriority.Render, new EventHandler(OnTimedEvent));
        aTimer.Start();
    }

    [ObservableProperty]
    private int maxDuration = 50;

    partial void OnMaxDurationChanged(int value)
    {
        if (MaxDuration < Duration) Duration = 0;
        if (aTimer.IsEnabled) aTimer.Stop();
        aTimer.Start();
    }

    [ObservableProperty]
    private int duration;

    [RelayCommand]
    private void OnReset()
    {
        Duration = 0;
        aTimer.Start();
    }

    private void OnTimedEvent(object? source, EventArgs e)
    {
        if (Duration < MaxDuration)
        {
            Duration++;
        }
        else
        {
            aTimer.Stop();
        }
    }
}
