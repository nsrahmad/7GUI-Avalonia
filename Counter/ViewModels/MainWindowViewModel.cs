using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace _1_Counter.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private int count;
    
    [RelayCommand]
    private void OnCountClicked() => Count++;
}
