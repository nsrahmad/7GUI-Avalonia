using System.Collections.ObjectModel;

using Avalonia.Controls;
using Avalonia.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CircleDrawer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<CircleViewModel> Circles { get; } = new();

    [ObservableProperty]
    private CircleViewModel? selectedCircle;

    partial void OnSelectedCircleChanging(CircleViewModel? value)
    {
        _ = value;
        if (SelectedCircle == null) return;
        SelectedCircle.IsSelected = false;
    }

    partial void OnSelectedCircleChanged(CircleViewModel? value) => value!.IsSelected = true;

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