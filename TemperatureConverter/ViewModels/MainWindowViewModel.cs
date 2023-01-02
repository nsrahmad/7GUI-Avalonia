using CommunityToolkit.Mvvm.ComponentModel;

namespace TemperatureConverter.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private double? temperatureCelsius;
}
