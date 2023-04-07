using System;
using System.Collections.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CircleDrawer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly UndoManager<ImmutableList<CircleViewModel>>
        undoManager = new(ImmutableList<CircleViewModel>.Empty);
    private CircleViewModel? beforeUpdateCircle;
    public ImmutableList<CircleViewModel>? Circles => undoManager.Current();
    

    [ObservableProperty] private CircleViewModel? selectedCircle;
    [ObservableProperty] private bool isDialogOpen;

    
    partial void OnSelectedCircleChanging(CircleViewModel? value)
    {
        _ = value;
        if (SelectedCircle == null) return;
        SelectedCircle.IsSelected = false;
    }

    partial void OnSelectedCircleChanged(CircleViewModel? value) => value!.IsSelected = true;
    
    // There is a lot of mental gymnastics here because of earlier decision
    // of how SelectedCircle is implemented.
    partial void OnIsDialogOpenChanged(bool value)
    {
        // value is true when the dialog opens
        if (value)
        {
            beforeUpdateCircle = new CircleViewModel()
            {
                Diameter = SelectedCircle!.Diameter,
                CenterX = SelectedCircle.CenterX,
                CenterY = SelectedCircle.CenterY,
            };
        }
        else // value is false when dialog is closing
        {
            var updatedCircle = new CircleViewModel()
            {
                Diameter = SelectedCircle!.Diameter,
                CenterX = SelectedCircle.CenterX,
                CenterY = SelectedCircle.CenterY,
            };
            
            var circle = undoManager.Current()!.FindLast(x => x == SelectedCircle);
            if (circle != null)
            {
                circle.Diameter = beforeUpdateCircle!.Diameter;
                circle.CenterX = beforeUpdateCircle.CenterX;
                circle.CenterY = beforeUpdateCircle.CenterY;
            }
            undoManager.Record(undoManager.Current()!.Replace(SelectedCircle!,updatedCircle));
            SelectedCircle = updatedCircle;
            OnPropertyChanged(nameof(Circles));
        }
    }

    [RelayCommand]
    private void OnAddCircle(Tuple<double, double>? coords)
    {
        if (coords == null) return;
        
        undoManager.Record(undoManager.Current()!.Add(
            new CircleViewModel
            {
                Diameter = 50,
                CenterX = coords.Item1,
                CenterY = coords.Item2,
            }
        ));
        OnPropertyChanged(nameof(Circles));
    }

    [RelayCommand]
    private void OnUndoButtonClicked()
    {
        undoManager.Undo();
        OnPropertyChanged(nameof(Circles));
    }

    [RelayCommand]
    private void OnRedoButtonClicked()
    {
        undoManager.Redo();
        OnPropertyChanged(nameof(Circles));
    }

    [RelayCommand]
    private void OnShowDialog() => IsDialogOpen = true;
}