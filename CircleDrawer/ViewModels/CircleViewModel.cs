using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CircleDrawer.ViewModels;

public partial class CircleViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(X))]
    [NotifyPropertyChangedFor(nameof(Y))]
    private double diameter = 50;
    
    public double X => CenterX - (Diameter / 2.0);

    public double Y => CenterY - (Diameter / 2.0);

    public double CenterX { get; set; }
    public double CenterY { get; set; }

    [ObservableProperty]
    private bool isSelected;
}