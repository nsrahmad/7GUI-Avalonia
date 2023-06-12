using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FlightBooker.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
{
    public MainWindowViewModel() => ValidateAllProperties();

    public static List<string> FlightType { get; } = new() { "one-way flight", "return flight" };

    [ObservableProperty]
    private string selectedFlightType = FlightType[0];

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnSelectedFlightTypeChanged(string value) => ValidateAllProperties();

    private DateTime? flightDate;

    [Required(ErrorMessage = "You must provide a valid flight date.")]
    public DateTime? FlightDate
    {
        get => flightDate;
        set
        {
            _ = SetProperty(ref flightDate, value, true);
            ValidateProperty(ReturnFlightDate, nameof(ReturnFlightDate));
        }
    }

    private DateTime? returnFlightDate;

    [CustomValidation(typeof(MainWindowViewModel), nameof(ValidateReturnFlightDate))]
    public DateTime? ReturnFlightDate
    {
        get => returnFlightDate;
        set => SetProperty(ref returnFlightDate, value, true);
    }

    public static ValidationResult ValidateReturnFlightDate(DateTime? returnFlightDate, ValidationContext context)
    {
        MainWindowViewModel instance = (MainWindowViewModel)context.ObjectInstance;

        return instance.SelectedFlightType.Equals("one-way flight")
            ? ValidationResult.Success!
            : returnFlightDate >= instance.flightDate
                ? ValidationResult.Success!
                : new("The return flight date must be a valid date equal to or after flight date.");
    }

    [ObservableProperty]
    private string? message;

    [RelayCommand]
    private void OnBookClicked() => Message = $"You have booked a {SelectedFlightType} on {FlightDate:dd/MM/yyyy}.";

    [RelayCommand]
    private void OnClearMessageClicked() => Message = null;
}
