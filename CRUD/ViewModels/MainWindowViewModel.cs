using Avalonia.Collections;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CRUD.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        _backingContacts = new()
        {
            new ("Hans", "Emil"),
            new ("Max", "Mustermann"),
            new ("Roman", "Tisch"),
        };
        contacts = new AvaloniaList<ContactViewModel>();
        UpdateContactsList();
    }

    private void UpdateContactsList()
    {
        foreach (ContactViewModel c in _backingContacts)
        {
            if (c.SurName!.StartsWith(FilterString ?? "", System.StringComparison.OrdinalIgnoreCase)) contacts.Add(c);
        }
    }

    private readonly AvaloniaList<ContactViewModel> _backingContacts;

    [ObservableProperty]
    private AvaloniaList<ContactViewModel> contacts;

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

    [ObservableProperty]
    private string? filterString = "m";

    partial void OnFilterStringChanged(string? value)
    {
        Contacts.Clear();
        UpdateContactsList();
    }

    [RelayCommand]
    private void OnDelete()
    {
        if (SelectedContact != null)
        {
            _ = Contacts.Remove(SelectedContact);
            _ = _backingContacts.Remove(SelectedContact);
        }
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
        if (TbName != null && TbSurName != null)
        {
            ContactViewModel c = new(TbName, TbSurName);
            Contacts.Add(c);
            _backingContacts.Add(c);
        }
    }
}
