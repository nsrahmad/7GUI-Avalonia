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

    [ObservableProperty] 
    private CircleViewModel? selectedCircle;

    [ObservableProperty] 
    private bool isDialogOpen;


    partial void OnSelectedCircleChanging(CircleViewModel? value)
    {
        _ = value;
        if (SelectedCircle == null) return;
        SelectedCircle.IsSelected = false;
    }

    partial void OnSelectedCircleChanged(CircleViewModel? value) => value!.IsSelected = true;


    private readonly CircleViewModel beforeUpdateCircle = new();

    partial void OnIsDialogOpenChanged(bool value)
    {
        // value is true when the dialog opens
        if (value)
        {
            beforeUpdateCircle.Diameter = SelectedCircle!.Diameter;
            beforeUpdateCircle.CenterX = SelectedCircle.CenterX;
            beforeUpdateCircle.CenterY = SelectedCircle.CenterY;

        }
        else // value is false when dialog is closing
        {
            CircleViewModel afterUpdateCircle = new()
            {
                Diameter = SelectedCircle!.Diameter,
                CenterX = SelectedCircle.CenterX,
                CenterY = SelectedCircle.CenterY
            };

            SelectedCircle.Diameter = beforeUpdateCircle.Diameter;
            SelectedCircle.CenterX = beforeUpdateCircle.CenterX;
            SelectedCircle.CenterY = beforeUpdateCircle.CenterY;

            Circles = UndoManager.Record(UndoManager.Current().Replace(SelectedCircle!, afterUpdateCircle));
            SelectedCircle = afterUpdateCircle;
            UndoManager.ResetRedo();
            RedoButtonClickedCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private void OnAddCircle(Tuple<double, double>? coords)
    {
        if (coords == null) return;

        Circles = UndoManager.Record(UndoManager.Current().Add(
            new CircleViewModel
            {
                Diameter = 50,
                CenterX = coords.Item1,
                CenterY = coords.Item2,
            }
        ));
        UndoManager.ResetRedo();
        RedoButtonClickedCommand.NotifyCanExecuteChanged();
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