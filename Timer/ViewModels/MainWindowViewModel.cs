using System.Timers;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Timer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly System.Timers.Timer aTimer;

    public MainWindowViewModel()
    {
        aTimer = new(1000);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Start();
    }

    [ObservableProperty]
    private int maxDuration = 50;

    partial void OnMaxDurationChanged(int value)
    {
        if (maxDuration < Duration) Duration = 0;
        if (aTimer.Enabled) aTimer.Stop();
        aTimer.Start();
    }

    [ObservableProperty]
    private int duration;

    [RelayCommand]
    private void OnReset()
    {
        Duration = 0;
        aTimer.Stop();
    }

    private void OnTimedEvent(object? source, ElapsedEventArgs e)
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
