using System;
using System.Collections.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CircleDrawer.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private static readonly UndoManager<ImmutableList<CircleViewModel>> UndoManager = new(ImmutableList<CircleViewModel>.Empty);

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UndoButtonClickedCommand))]
    [NotifyCanExecuteChangedFor(nameof(RedoButtonClickedCommand))]
    private ImmutableList<CircleViewModel> circles = UndoManager.Current();

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

    private static bool CanUndo() => UndoManager.IsUndoAvailable();
    private static bool CanRedo() => UndoManager.IsRedoAvailable();

    [RelayCommand(CanExecute = nameof(CanUndo))]
    private void OnUndoButtonClicked() => Circles = UndoManager.Undo();

    [RelayCommand(CanExecute = nameof(CanRedo))]
    private void OnRedoButtonClicked() => Circles = UndoManager.Redo();

    [RelayCommand]
    private void OnShowDialog() => IsDialogOpen = true;
}