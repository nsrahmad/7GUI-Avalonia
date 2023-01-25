using System;

using Avalonia.Collections;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CRUD.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private AvaloniaList<ContactViewModel> contacts = new()
    {
        new ("Hans", "Emil"),
        new ("Max", "Mustermann"),
        new ("Roman", "Tisch"),
    };
    
    [ObservableProperty]
    private ContactViewModel? selectedContact;

    partial void OnSelectedContactChanged(ContactViewModel? value)
    {
        if (value != null)
        {
            TbName = value.Name;
            TbSurName = value.SurName;
        }
    }

    [ObservableProperty]
    private string? tbName;

    [ObservableProperty]
    private string? tbSurName;

    [RelayCommand]
    private void OnDelete()
    {
        if (SelectedContact != null) Contacts.Remove(SelectedContact);
    }

    [RelayCommand]
    private void OnUpdate()
    {
        if (SelectedContact != null)
        {
            SelectedContact.Name = TbName;
            SelectedContact.SurName = TbSurName;
        }
    }

    [RelayCommand]
    private void OnCreate()
    {
        if (TbName != null && TbSurName != null) Contacts.Add(new ContactViewModel(TbName, TbSurName));
    }
}
