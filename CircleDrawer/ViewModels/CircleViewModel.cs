using CommunityToolkit.Mvvm.ComponentModel;

namespace CircleDrawer.ViewModels;

public partial class CircleViewModel : ObservableObject
{
    public double Diameter { get; set; }

    public double X => CenterX - (Diameter / 2.0);

    public double Y => CenterY - (Diameter / 2.0);

    public double CenterX { get; set; }
    public double CenterY { get; set; }

    [ObservableProperty]
    private string fillColor = "White";
}