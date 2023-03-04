using System;
using System.Collections.ObjectModel;

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
    private void OnAddCircle(Tuple<double, double>? coords)
    {
        if (coords != null)
        {
            Circles.Add(new CircleViewModel()
            {
                Diameter = 50, CenterX = coords.Item1, CenterY = coords.Item2,
            });
        }
    }
}