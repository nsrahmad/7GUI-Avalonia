using System.Linq;

using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CircleDrawer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    public AvaloniaList<CircleViewModel> Circles { get; } = new()
    {
        new CircleViewModel()
        {
            Diameter = 200, CenterX = 0, CenterY = 0,
        },
        new CircleViewModel()
        {
            Diameter = 100, CenterX = 0, CenterY = 0,
        },
        new CircleViewModel()
        {
            Diameter = 50, CenterX = 0, CenterY = 0,
        }
    };

    [ObservableProperty]
    private CircleViewModel? selectedCircle;

    partial void OnSelectedCircleChanged(CircleViewModel? value)
    {
        foreach (CircleViewModel circle in Circles.Where(circle => circle.FillColor.Equals("LightGray")))
        {
            circle.FillColor = "White";
        }
        if (value != null) value.FillColor = "LightGray";
    }

    [RelayCommand]
    private void OnAddCircle(PointerPressedEventArgs e)
    {
        if (e.Source is Canvas canvas)
        {
            Circles.Add(new CircleViewModel()
            {
                Diameter = 50,
                CenterX = e.GetCurrentPoint(canvas).Position.X,
                CenterY = e.GetCurrentPoint(canvas).Position.Y,
            });
        }
    }
}