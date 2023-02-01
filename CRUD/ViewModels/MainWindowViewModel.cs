using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Platform;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CRUD.Models;

using DynamicData;

namespace CRUD.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    public MainWindowViewModel()
    {
        backingContacts.AddRange(LoadData());

        list = backingContacts.Connect()
            .Filter(filter)
            .Bind(out contacts)
            .DisposeMany()
            .Subscribe();

        filter.OnNext(CreateFilter());
    }

    private readonly IDisposable list;
    private readonly Subject<Func<ObservableContact, bool>> filter = new();
    private readonly SourceList<ObservableContact> backingContacts = new();

    private Func<ObservableContact, bool> CreateFilter() => c => c.SurName.StartsWith(FilterString, StringComparison.OrdinalIgnoreCase);

    private readonly ReadOnlyObservableCollection<ObservableContact> contacts;
    public ReadOnlyObservableCollection<ObservableContact> Contacts => contacts;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    private ObservableContact? selectedContact;

    partial void OnSelectedContactChanged(ObservableContact? value)
    {
        if (value != null)
        {
            TbName = value.Name;
            TbSurName = value.SurName;
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string? tbName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string? tbSurName;

    private bool IsNameNotNull => !string.IsNullOrWhiteSpace(TbName) && !string.IsNullOrWhiteSpace(TbSurName);

    [ObservableProperty]
    private string filterString = string.Empty;

    partial void OnFilterStringChanged(string value) => filter.OnNext(CreateFilter());

    private bool IsSelectedContact => SelectedContact != null;

    [RelayCommand(CanExecute = nameof(IsSelectedContact))]
    private void OnDelete()
    {
        _ = backingContacts.Remove(SelectedContact!);
    }

    [RelayCommand(CanExecute = nameof(IsSelectedContact))]
    private void OnUpdate()
    {
        if (TbName != null && TbSurName != null)
        {
            SelectedContact!.Name = TbName;
            SelectedContact.SurName = TbSurName;
        }
    }

    [RelayCommand(CanExecute = nameof(IsNameNotNull))]
    private void OnCreate()
    {
        // We are sure TbName and TbSurName can not be null here
        backingContacts.Add(new ObservableContact(TbName!, TbSurName!));
    }

    private static List<ObservableContact> LoadData()
    {
        IAssetLoader? assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        using Stream contactsOnDisk = assets!.Open(new System.Uri("avares://CRUD/Assets/MOCK_DATA.csv"));
        using StreamReader reader = new(contactsOnDisk);
        List<ObservableContact> contacts = new();
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] contact = line.Split(',');
            contacts.Add(new ObservableContact(contact[0], contact[1]));
        }
        return contacts;
    }

    public void Dispose()
    {
        list.Dispose();
        filter.Dispose();
        backingContacts.Dispose();
    }
}
